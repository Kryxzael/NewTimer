using CleanNodeTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.Forms
{
    /// <summary>
    /// Represents a way to create and load save files to and from a setup form
    /// </summary>
    public partial class Setup
    {
        /// <summary>
        /// Saves a to-time preset to disk
        /// </summary>
        /// <param name="path">Location to write to</param>
        public void SaveTimeToFile(string path)
        {
            try
            {
                CreateNodeFromToTime().ToFile(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while saving this file:\r\n" + ex.Message, "Save file error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Saves a duration preset to disk
        /// </summary>
        /// <param name="path">Location to write to</param>
        public void SaveDurationToFile(string path)
        {
            try
            {
                CreateNodeFromDuration().ToFile(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while saving this file:\r\n" + ex.Message, "Save file error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the given file path and initializes the setup controls with its contents
        /// </summary>
        /// <param name="path">Path to the file to load</param>
        public void LoadFile(string path)
        {
            try
            {
                /*
                 * Read preset file
                 */
                HierarchyNode node;

                try
                {
                    node = HierarchyNode.FromFile(path);
                }
                catch (Exception)
                {
                    MessageBox.Show("The specified file was not found or could not be openend!", "Open file error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Preset is of type: to-time
                if (node["PresetType"].String == "toTime")
                {
                    //Navigate to to-time screen
                    tabs.SelectedIndex = 0;

                    //Load advanced-mode switch
                    chkAdv.Checked = node["EnableExtendedSelector"].Boolean;

                    //Load date values
                    if (node["EnableExtendedSelector"].Boolean)
                    {
                        numYear.Value = node["Target;Date;Year"].Int;
                        knbMonth.Value = node["Target;Date;Month"].Int;
                        knbDay.Value = node["Target;Date;Day"].Int;
                        knbSec.Value = node["Target;Time;Second"].Int;
                    }

                    //Load time values
                    knbHour.Value = node["Target;Time;Hour"].Int;
                    knbMin.Value = node["Target;Time;Minute"].Int;

                    //Load Stop At Zero
                    chkStopAtZero.Checked = node["StopAtZero"]?.Boolean ?? false;
                }

                //Preset is of type: duration
                else if (node["PresetType"].String == "duration")
                {
                    //Navigate to duration screen
                    tabs.SelectedIndex = 1;

                    //Load time values
                    knbDurHour.Value = node["Target;Hours"].Int;
                    knbDurMin.Value = node["Target;Minutes"].Int;
                    knbDurSec.Value = node["Target;Seconds"].Int;

                    //Load Stop At Zero
                    chkDurStopAtZero.Checked = node["StopAtZero"]?.Boolean ?? false;
                }

                //The preset did not have a valid header
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid preset type!");
                }

                //Load the color scheme
                cboxColors.SelectedIndex = node["ColorScheme"]?.Int ?? 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while loading this file:\r\n" + ex.Message, "Open file error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Creates a hierarchy node instance from the data in the to-time tab
        /// </summary>
        /// <returns></returns>
        protected HierarchyNode CreateNodeFromToTime()
        {
            HierarchyNode node = new HierarchyNode("CountdownPreset");

            //We are saving a to-time preset
            //Create header
            node.Add("PresetType", "toTime");

            //Advanced mode
            node.Add("EnableExtendedSelector", chkAdv.Checked);

            //Create target structure
            HierarchyNode targetNode = node.Add("Target");
            {
                if (chkAdv.Checked)
                {
                    //Date node
                    targetNode.Add("Date");
                    {
                        targetNode["Date"].Add("Year", numYear.Value);
                        targetNode["Date"].Add("Month", knbMonth.Value);
                        targetNode["Date"].Add("Day", knbDay.Value);
                    }

                }

                //Time node
                targetNode.Add("Time");
                {
                    targetNode["Time"].Add("Hour", knbHour.Value);
                    targetNode["Time"].Add("Minute", knbMin.Value);
                    if (chkAdv.Checked) targetNode["Time"].Add("Second", knbSec.Value);
                }
            }

            //Create color scheme node
            node.Add("ColorScheme", cboxColors.SelectedIndex);

            //Create Stop at Zero node
            node.Add("StopAtZero", chkStopAtZero.Checked);
            return node;
        }

        /// <summary>
        /// Creates a hierarchy node instance from the data in the duration tab
        /// </summary>
        /// <returns></returns>
        protected HierarchyNode CreateNodeFromDuration()
        {
            HierarchyNode node = new HierarchyNode("CountdownPreset");

            //Create header
            node.Add("PresetType", "duration");

            //Create target structure
            HierarchyNode targetNode = node.Add("Target");
            {
                targetNode.Add("Hours", knbDurHour.Value);
                targetNode.Add("Minutes", knbDurMin.Value);
                targetNode.Add("Seconds", knbDurSec.Value);
            }

            //Create color scheme node
            node.Add("ColorScheme", cboxColors.SelectedIndex);

            //Create Stop at Zero node
            node.Add("StopAtZero", chkDurStopAtZero.Checked);
            return node;
        }
    }
}
