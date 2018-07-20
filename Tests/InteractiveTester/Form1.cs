using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmEntityDebuggerVisualizer;

namespace InteractiveTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bEntityCollection1_Click(object sender, EventArgs e)
        {
            EntityCollection ecoll = new EntityCollection
            {
                EntityName = "test entity",
                Entities =
                {
                    new Entity{ Id=Guid.NewGuid() },
                    new Entity{ Id=Guid.NewGuid() }
                }
            };
            ecoll.Entities[0].Attributes.Add("test", "ąęć");
            ecoll.Entities[0].Attributes.Add("test2", null);

            ecoll.Entities[1].Attributes.Add("test2", "ala ma kota");
            ecoll.Entities[1].Attributes.Add("test3", new EntityReference("account", Guid.NewGuid()));

            EntityCollectionVisualizer.TestShowVisualizer(ecoll);
        }
    }
}
