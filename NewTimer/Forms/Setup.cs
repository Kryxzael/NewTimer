using CleanNodeTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.Forms
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
            knbHour.Value = DateTime.Now.Hour + 1 % 24;

            knbMonth.ValueChanged += (s, e) => knbDay.MaxValue = DateTime.DaysInMonth((int)numYear.Value, knbMonth.Value);
            btnStartTime.MouseDown += OnClickStart;
            btnStartDuration.MouseDown += OnClickStart;
            FormClosing += HandleClose;
            numYear.Minimum = DateTime.Now.Year - 1;
            numYear.Maximum = DateTime.Now.Year + 1;
            ChkAdv_CheckedChanged(null, null);

            foreach (FormParts.Setup.Knob i in tabTime.Controls.Cast<Control>().Where(i => i is FormParts.Setup.Knob))
            {
                i.ValueChanged += (s, e) => CreateSuggestions();
            }

            foreach (FormParts.Setup.Knob i in tabTime.Controls.Cast<Control>().Where(i => i is FormParts.Setup.Knob))
            {
                i.ValueChanged += (s, e) => CreateSuggestions();
            }

            CreateSuggestions();
            colorSchemeComboBox1.Items.Add("Hello");
        }

        private void HandleClose(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.None)
            {
                Application.Exit();
            }
        }

        private void OnClickStart(object sender, MouseEventArgs e)
        {
            if (sender == btnStartTime)
            {
                Config.StartTimer(new DateTime((int)numYear.Value, knbMonth.Value, knbDay.Value, knbHour.Value, knbMin.Value, knbSec.Value), this);
            }
            else if (sender == btnStartDuration)
            {
                Config.StartTimer(DateTime.Now.Add(new TimeSpan(knbDurHour.Value, knbDurMin.Value, knbDurSec.Value)), this);
            }
        }

        private void CreateSuggestions()
        {
            flwTimeSuggestions.Controls.Clear();
            flwTimeSuggestionsDuration.Controls.Clear();

            int h = knbHour.Value;
            int m = knbMin.Value;

            //Smart next xx:mm
            if (DateTime.Now.Minute >= m)
            {
                createTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, m, 0).AddHours(1));
            }
            else
            {
                createTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, m, 0));
            }

            //Smart next hh:mm
            if (DateTime.Now.TimeOfDay > new TimeSpan(h, m, 0))
            {
                createTimeWithText(h.ToString("00") + ":" + m.ToString("00") + " tomorrow", DateTime.Now.Date.AddHours(h).AddMinutes(m).AddDays(1));
            }
            else
            {
                createTimeWithText(h.ToString("00") + ":" + m.ToString("00") + " today", DateTime.Now.Date.AddHours(h).AddMinutes(m));
            }

            switch (DateTime.Now.Minute)
            {
                case int i when i < 15:
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 0.25));
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 0.5f));
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1));
                    break;
                case int i when i < 30:
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 0.5f));
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 0.75f));
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1f));
                    break;
                case int i when i < 45:
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 0.75f));
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1f));
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1.5f));
                    break;
                default:
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1));
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 1.5f));
                    createTime(DateTime.Now.Date.AddHours(DateTime.Now.Hour + 2));
                    break;
            }

            createTimeDuration("30s", DateTime.Now.AddSeconds(30));
            createTimeDuration("1m", DateTime.Now.AddMinutes(1));
            createTimeDuration("3m", DateTime.Now.AddMinutes(3));
            createTimeDuration("5m", DateTime.Now.AddMinutes(5));
            createTimeDuration("10m", DateTime.Now.AddMinutes(10));
            createTimeDuration("15m", DateTime.Now.AddMinutes(15));
            createTimeDuration("20m", DateTime.Now.AddMinutes(20));
            createTimeDuration("30m", DateTime.Now.AddMinutes(30));
            createTimeDuration("45m", DateTime.Now.AddMinutes(45));
            createTimeDuration("1h", DateTime.Now.AddHours(1));
            createTimeDuration("2h", DateTime.Now.AddHours(2));
            createTimeDuration("3h", DateTime.Now.AddHours(3));

            void createTime(DateTime target)
            {
                flwTimeSuggestions.Controls.Add(new FormParts.Setup.TimeSugestion(target.ToShortTimeString(), target));
            }

            void createTimeWithText(string text, DateTime target)
            {
                flwTimeSuggestions.Controls.Add(new FormParts.Setup.TimeSugestion(text, target));
            }

            void createTimeDuration(string text, DateTime target)
            {
                flwTimeSuggestionsDuration.Controls.Add(new FormParts.Setup.TimeSugestion(text, target));
            }
        }

        private void ChkAdv_CheckedChanged(object sender, EventArgs e)
        {
            numYear.Value = DateTime.Now.Year;
            knbMonth.Value = DateTime.Now.Month;
            knbDay.Value = DateTime.Now.Day;
            knbSec.Value = 0;

            if (chkAdv.Checked)
            {
                knbDay.Show();
                knbMonth.Show();
                knbSec.Show();
                numYear.Show();
                lblYear.Visible = true;
                chkAdv.Location = new Point(305, 131);
                knbMin.Step = 1;
            }
            else
            {
                knbDay.Hide();
                knbMonth.Hide();
                knbSec.Hide();
                numYear.Hide();
                lblYear.Visible = false;
                chkAdv.Location = new Point(305, 6);
                knbMin.Step = 5;
            }
        }

        private void OnClickSave(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Countdown preset|*.countdownpreset",
                FileName = "Preset.countdownpreset"
            };

            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            HierarchyNode node = new HierarchyNode("CountdownPreset");

            if (sender == btnSaveCountdown)
            {
                node.Add("PresetType", "toTime");
                HierarchyNode targetNode = node.Add("Target");

                node.Add("EnableExtendedSelector", chkAdv.Checked);

                if (chkAdv.Checked)
                {
                    targetNode.Add("Date");
                    targetNode["Date"].Add("Year", numYear.Value);
                    targetNode["Date"].Add("Month", knbMonth.Value);
                    targetNode["Date"].Add("Day", knbDay.Value);
                }

                targetNode.Add("Time");
                targetNode["Time"].Add("Hour", knbHour.Value);
                targetNode["Time"].Add("Minute", knbMin.Value);

                if (chkAdv.Checked)
                {
                    targetNode["Time"].Add("Second", knbSec.Value);
                }

            }
            else if (sender == btnSaveDuration)
            {
                node.Add("PresetType", "duration");
                HierarchyNode targetNode = node.Add("Target");
                targetNode.Add("Hours", knbDurHour.Value);
                targetNode.Add("Minutes", knbDurMin.Value);
                targetNode.Add("Seconds", knbDurSec.Value);
            }

            try
            {
                node.ToFile(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while saving this file:\r\n" + ex.Message, "Save file error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void OnClickLoad(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Countdown preset|*.countdownpreset"
            };

            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            LoadFile(dialog.FileName);
            

        }

        public void LoadFile(string path)
        {
            try
            {
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

                if (node["PresetType"].String == "toTime")
                {
                    tabs.SelectedIndex = 0;

                    chkAdv.Checked = node["EnableExtendedSelector"].Boolean;

                    if (node["EnableExtendedSelector"].Boolean)
                    {
                        numYear.Value = node["Target;Date;Year"].Int;
                        knbMonth.Value = node["Target;Date;Month"].Int;
                        knbDay.Value = node["Target;Date;Day"].Int;
                        knbSec.Value = node["Target;Time;Second"].Int;
                    }

                    knbHour.Value = node["Target;Time;Hour"].Int;
                    knbMin.Value = node["Target;Time;Minute"].Int;
                    btnStartTime.PerformClick();

                }
                else if (node["PresetType"].String == "duration")
                {
                    tabs.SelectedIndex = 1;

                    knbDurHour.Value = node["Target;Hours"].Int;
                    knbDurMin.Value = node["Target;Minutes"].Int;
                    knbDurSec.Value = node["Target;Seconds"].Int;
                    btnStartDuration.PerformClick();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid preset type!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while loading this file:\r\n" + ex.Message, "Open file error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
