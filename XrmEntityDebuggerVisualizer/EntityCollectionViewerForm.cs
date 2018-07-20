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

namespace XrmEntityDebuggerVisualizer
{
    public partial class EntityCollectionViewerForm : Form
    {
        public EntityCollectionViewerForm(EntityCollection entityCollection)
        {
            InitializeComponent();

            AddToGrid(entityCollection);
        }

        private void EntityCollectionViewerForm_Load(object sender, EventArgs e)
        {

        }

        private void AddToGrid(EntityCollection entityCollection)
        {    
            DataTable dataTable = EntityCollectionToDataTable(entityCollection);

            grid.DataSource = dataTable;
        }

        public DataTable EntityCollectionToDataTable(EntityCollection ecoll)
        {
            DataTable dataTable = new DataTable();

            if (ecoll?.Entities == null)
            {
                return dataTable;
            }

            foreach (Entity entity in ecoll.Entities)
            {
                DataRow row = dataTable.NewRow();
          
                foreach (string attributeName in entity.Attributes?.Keys?.ToArray())
                {                  
                    string stringValue = CrmAttributeToString(entity[attributeName]);
                    if (!dataTable.Columns.Contains(attributeName))
                    {
                        dataTable.Columns.Add(attributeName, typeof(string));
                    }
                    row[attributeName] = stringValue;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        private string CrmAttributeToString(object attributeValue)
        {
            if (attributeValue == null)
            {
                return "";
            }

            if(attributeValue is EntityReference entityReference)
            {
                return $"{entityReference.LogicalName}\\{entityReference.Id}";
            }
            if(attributeValue is OptionSetValue optionSetValue)
            {
                return $"{optionSetValue.Value}";
            }
            if (attributeValue is Money money)
            {
                return $"{money.Value}";
            }
            if (attributeValue is AliasedValue aliasedValue)
            {
                return CrmAttributeToString(aliasedValue.Value);
            }

            return attributeValue.ToString();
        }
    }
}
