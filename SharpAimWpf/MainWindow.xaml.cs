using System;
using System.Linq;
using System.Windows;
using Binarysharp.MemoryManagement;
using SharpAimWpf.SharpAim;

namespace SharpAimWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MemorySharp _process;
        private readonly Player _player;
        private readonly Entity _entity;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                _process = new MemorySharp(System.Diagnostics.Process.GetProcessesByName("csgo").FirstOrDefault());
            }
            catch (Exception e)
            {
                MessageBox.Show($"CS:GO is not running, please start CS:GO before attempting to run this program.\n\n {e}",
                    "CS:GO not running!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(1);
            }

            _player = new Player(_process);
            _entity = new Entity(_process);

            var entities = Entity.AllEntities(_process);

            var count = 0;

            foreach (var entity in entities)
            {
                var distanceToPlayer = General.GetDistanceToEnemyFromPlayer(_player.Position, entity.Position);

                lbxEntity.Items.Add($"({count})Entity Address: 0x{entity.EntityAddr:X8}, Distance: {distanceToPlayer:.##}m");
                count++;
            }

            /*
            TEST STUFF
            */

            /*var entityList = new List<Entity>();

            for (var i = 0; i < 32; i++)
            {
                try
                {
                    _entity.SingleEntity(i);

                    if (_entity.EntityAddr != 0)
                        entityList.Add(_entity);

                }
                catch (Exception)
                {

                }
            }

            foreach (var entity in entityList)
            {
                lbxEntity.Items.Add(entity.Name + $" {entity.RadarNameAddress:X8}");
            }*/
        }

        public string HotkeyText => "Hotkeys\n" +
                                    "Numpad 1: Trigger\n" +
                                    "Numpad 2: Glow";
    }
}
