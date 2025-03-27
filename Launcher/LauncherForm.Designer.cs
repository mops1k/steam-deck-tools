using Launcher.Helper;
using System.ComponentModel;
using System.Windows;
namespace Launcher
{
    partial class LauncherForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
            listControlPanel = new System.Windows.Forms.Panel();
            powerLabel = new System.Windows.Forms.Label();
            powerImage = new System.Windows.Forms.PictureBox();
            controllerLabel = new System.Windows.Forms.Label();
            controllerImage = new System.Windows.Forms.PictureBox();
            performanceLabel = new System.Windows.Forms.Label();
            performanceImage = new System.Windows.Forms.PictureBox();
            fanLabel = new System.Windows.Forms.Label();
            fanImage = new System.Windows.Forms.PictureBox();
            powerSwitch = new AT.WinForm.ToggleSwitch();
            controllerSwitch = new AT.WinForm.ToggleSwitch();
            performanceSwitch = new AT.WinForm.ToggleSwitch();
            fanSwitch = new AT.WinForm.ToggleSwitch();
            startButton = new System.Windows.Forms.Button();
            stopButton = new System.Windows.Forms.Button();
            restartButton = new System.Windows.Forms.Button();
            shortcutButton = new System.Windows.Forms.Button();
            listControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)powerImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)controllerImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)performanceImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fanImage).BeginInit();
            SuspendLayout();
            // 
            // listControlPanel
            // 
            listControlPanel.BackgroundImage = global::Launcher.Resources.bg;
            listControlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            listControlPanel.Controls.Add(powerLabel);
            listControlPanel.Controls.Add(powerImage);
            listControlPanel.Controls.Add(controllerLabel);
            listControlPanel.Controls.Add(controllerImage);
            listControlPanel.Controls.Add(performanceLabel);
            listControlPanel.Controls.Add(performanceImage);
            listControlPanel.Controls.Add(fanLabel);
            listControlPanel.Controls.Add(fanImage);
            listControlPanel.Controls.Add(powerSwitch);
            listControlPanel.Controls.Add(controllerSwitch);
            listControlPanel.Controls.Add(performanceSwitch);
            listControlPanel.Controls.Add(fanSwitch);
            listControlPanel.Location = new System.Drawing.Point(10, 10);
            listControlPanel.Margin = new System.Windows.Forms.Padding(0);
            listControlPanel.Name = "listControlPanel";
            listControlPanel.Size = new System.Drawing.Size(330, 250);
            listControlPanel.TabIndex = 0;
            // 
            // powerLabel
            // 
            powerLabel.AutoSize = true;
            powerLabel.BackColor = System.Drawing.Color.Transparent;
            powerLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            powerLabel.Location = new System.Drawing.Point(47, 212);
            powerLabel.Name = "powerLabel";
            powerLabel.Size = new System.Drawing.Size(124, 18);
            powerLabel.TabIndex = 15;
            powerLabel.Text = "Power Control";
            // 
            // powerImage
            // 
            powerImage.BackColor = System.Drawing.Color.Transparent;
            powerImage.Image = global::Launcher.Resources.traffic_light_outline;
            powerImage.Location = new System.Drawing.Point(15, 209);
            powerImage.Name = "powerImage";
            powerImage.Size = new System.Drawing.Size(24, 24);
            powerImage.TabIndex = 14;
            powerImage.TabStop = false;
            // 
            // controllerLabel
            // 
            controllerLabel.AutoSize = true;
            controllerLabel.BackColor = System.Drawing.Color.Transparent;
            controllerLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            controllerLabel.Location = new System.Drawing.Point(47, 148);
            controllerLabel.Name = "controllerLabel";
            controllerLabel.Size = new System.Drawing.Size(147, 18);
            controllerLabel.TabIndex = 13;
            controllerLabel.Text = "Steam Controller";
            // 
            // controllerImage
            // 
            controllerImage.BackColor = System.Drawing.Color.Transparent;
            controllerImage.Image = global::Launcher.Resources.microsoft_xbox_controller;
            controllerImage.Location = new System.Drawing.Point(15, 145);
            controllerImage.Name = "controllerImage";
            controllerImage.Size = new System.Drawing.Size(24, 24);
            controllerImage.TabIndex = 12;
            controllerImage.TabStop = false;
            // 
            // performanceLabel
            // 
            performanceLabel.AutoSize = true;
            performanceLabel.BackColor = System.Drawing.Color.Transparent;
            performanceLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            performanceLabel.Location = new System.Drawing.Point(47, 85);
            performanceLabel.Name = "performanceLabel";
            performanceLabel.Size = new System.Drawing.Size(182, 18);
            performanceLabel.TabIndex = 11;
            performanceLabel.Text = "Performance Overlay";
            // 
            // performanceImage
            // 
            performanceImage.BackColor = System.Drawing.Color.Transparent;
            performanceImage.Image = global::Launcher.Resources.poll;
            performanceImage.Location = new System.Drawing.Point(15, 82);
            performanceImage.Name = "performanceImage";
            performanceImage.Size = new System.Drawing.Size(24, 24);
            performanceImage.TabIndex = 10;
            performanceImage.TabStop = false;
            // 
            // fanLabel
            // 
            fanLabel.AutoSize = true;
            fanLabel.BackColor = System.Drawing.Color.Transparent;
            fanLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            fanLabel.Location = new System.Drawing.Point(47, 22);
            fanLabel.Name = "fanLabel";
            fanLabel.Size = new System.Drawing.Size(97, 18);
            fanLabel.TabIndex = 9;
            fanLabel.Text = "FanControl";
            // 
            // fanImage
            // 
            fanImage.BackColor = System.Drawing.Color.Transparent;
            fanImage.Image = global::Launcher.Resources.fan;
            fanImage.Location = new System.Drawing.Point(15, 19);
            fanImage.Name = "fanImage";
            fanImage.Size = new System.Drawing.Size(24, 24);
            fanImage.TabIndex = 8;
            fanImage.TabStop = false;
            // 
            // powerSwitch
            // 
            powerSwitch.Location = new System.Drawing.Point(240, 206);
            powerSwitch.Name = "powerSwitch";
            powerSwitch.OffFont = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            powerSwitch.OffForeColor = System.Drawing.Color.WhiteSmoke;
            powerSwitch.OffText = "OFF";
            powerSwitch.OnFont = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            powerSwitch.OnForeColor = System.Drawing.Color.WhiteSmoke;
            powerSwitch.OnText = "ON";
            powerSwitch.Size = new System.Drawing.Size(80, 30);
            powerSwitch.Style = AT.WinForm.ToggleSwitch.ToggleSwitchStyle.Modern;
            powerSwitch.TabIndex = 7;
            powerSwitch.CheckedChanged += powerSwitch_CheckedChanged;
            // 
            // controllerSwitch
            // 
            controllerSwitch.Location = new System.Drawing.Point(240, 142);
            controllerSwitch.Name = "controllerSwitch";
            controllerSwitch.OffFont = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            controllerSwitch.OffForeColor = System.Drawing.Color.WhiteSmoke;
            controllerSwitch.OffText = "OFF";
            controllerSwitch.OnFont = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            controllerSwitch.OnForeColor = System.Drawing.Color.WhiteSmoke;
            controllerSwitch.OnText = "ON";
            controllerSwitch.Size = new System.Drawing.Size(80, 30);
            controllerSwitch.Style = AT.WinForm.ToggleSwitch.ToggleSwitchStyle.Modern;
            controllerSwitch.TabIndex = 5;
            controllerSwitch.CheckedChanged += controllerSwitch_CheckedChanged;
            // 
            // performanceSwitch
            // 
            performanceSwitch.Location = new System.Drawing.Point(240, 78);
            performanceSwitch.Name = "performanceSwitch";
            performanceSwitch.OffFont = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            performanceSwitch.OffForeColor = System.Drawing.Color.WhiteSmoke;
            performanceSwitch.OffText = "OFF";
            performanceSwitch.OnFont = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            performanceSwitch.OnForeColor = System.Drawing.Color.WhiteSmoke;
            performanceSwitch.OnText = "ON";
            performanceSwitch.Size = new System.Drawing.Size(80, 30);
            performanceSwitch.Style = AT.WinForm.ToggleSwitch.ToggleSwitchStyle.Modern;
            performanceSwitch.TabIndex = 3;
            performanceSwitch.CheckedChanged += performanceSwitch_CheckedChanged;
            // 
            // fanSwitch
            // 
            fanSwitch.Location = new System.Drawing.Point(240, 17);
            fanSwitch.Name = "fanSwitch";
            fanSwitch.OffFont = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            fanSwitch.OffForeColor = System.Drawing.Color.WhiteSmoke;
            fanSwitch.OffText = "OFF";
            fanSwitch.OnFont = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            fanSwitch.OnForeColor = System.Drawing.Color.WhiteSmoke;
            fanSwitch.OnText = "ON";
            fanSwitch.Size = new System.Drawing.Size(80, 30);
            fanSwitch.Style = AT.WinForm.ToggleSwitch.ToggleSwitchStyle.Modern;
            fanSwitch.TabIndex = 1;
            fanSwitch.CheckedChanged += fanSwitch_CheckedChanged;
            // 
            // startButton
            // 
            startButton.BackColor = System.Drawing.Color.FromArgb(((int)((byte)0)), ((int)((byte)191)), ((int)((byte)158)));
            startButton.FlatAppearance.BorderSize = 0;
            startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            startButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            startButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            startButton.Image = global::Launcher.Resources.Start;
            startButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            startButton.Location = new System.Drawing.Point(10, 266);
            startButton.Name = "startButton";
            startButton.Padding = new System.Windows.Forms.Padding(10);
            startButton.Size = new System.Drawing.Size(133, 53);
            startButton.TabIndex = 1;
            startButton.Text = "START";
            startButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            startButton.UseVisualStyleBackColor = false;
            startButton.Click += startButton_Click;
            startButton.MouseLeave += startButton_MouseLeave;
            startButton.MouseHover += startButton_MouseHover;
            // 
            // stopButton
            // 
            stopButton.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)58)), ((int)((byte)104)));
            stopButton.FlatAppearance.BorderSize = 0;
            stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            stopButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            stopButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            stopButton.Image = global::Launcher.Resources.Stop;
            stopButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            stopButton.Location = new System.Drawing.Point(149, 266);
            stopButton.Name = "stopButton";
            stopButton.Padding = new System.Windows.Forms.Padding(10);
            stopButton.Size = new System.Drawing.Size(133, 53);
            stopButton.TabIndex = 2;
            stopButton.Text = "STOP";
            stopButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            stopButton.UseVisualStyleBackColor = false;
            stopButton.Click += stopButton_Click;
            stopButton.MouseLeave += stopButton_MouseLeave;
            stopButton.MouseHover += stopButton_MouseHover;
            // 
            // restartButton
            // 
            restartButton.BackColor = System.Drawing.Color.FromArgb(((int)((byte)0)), ((int)((byte)191)), ((int)((byte)253)));
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            restartButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            restartButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            restartButton.Image = global::Launcher.Resources.Restart;
            restartButton.Location = new System.Drawing.Point(287, 266);
            restartButton.Name = "restartButton";
            restartButton.Padding = new System.Windows.Forms.Padding(10);
            restartButton.Size = new System.Drawing.Size(53, 53);
            restartButton.TabIndex = 3;
            restartButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            restartButton.UseVisualStyleBackColor = false;
            restartButton.Click += restartButton_Click;
            restartButton.MouseLeave += restartButton_MouseLeave;
            restartButton.MouseHover += restartButton_MouseHover;
            // 
            // shortcutButton
            // 
            shortcutButton.BackColor = System.Drawing.Color.Black;
            shortcutButton.FlatAppearance.BorderSize = 0;
            shortcutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            shortcutButton.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
            shortcutButton.ForeColor = System.Drawing.Color.WhiteSmoke;
            shortcutButton.Image = global::Launcher.Resources.Link_16x16;
            shortcutButton.Location = new System.Drawing.Point(10, 325);
            shortcutButton.Name = "shortcutButton";
            shortcutButton.Size = new System.Drawing.Size(330, 26);
            shortcutButton.TabIndex = 4;
            shortcutButton.Text = "GENERATE DESKTOP SHORTCUTS";
            shortcutButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            shortcutButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            shortcutButton.UseVisualStyleBackColor = false;
            shortcutButton.Click += shortcutButton_Click;
            // 
            // LauncherForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(((int)((byte)227)), ((int)((byte)229)), ((int)((byte)240)));
            ClientSize = new System.Drawing.Size(352, 361);
            Controls.Add(shortcutButton);
            Controls.Add(restartButton);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Controls.Add(listControlPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Manage SteamDeck Tools";
            listControlPanel.ResumeLayout(false);
            listControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)powerImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)controllerImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)performanceImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)fanImage).EndInit();
            ResumeLayout(false);
        }
        private System.Windows.Forms.Label performanceLabel;
        private System.Windows.Forms.PictureBox performanceImage;
        private System.Windows.Forms.Label controllerLabel;
        private System.Windows.Forms.PictureBox controllerImage;
        private System.Windows.Forms.Label powerLabel;
        private System.Windows.Forms.PictureBox powerImage;
        private System.Windows.Forms.Label fanLabel;
        private System.Windows.Forms.PictureBox fanImage;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button shortcutButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Button stopButton;
        private AT.WinForm.ToggleSwitch powerSwitch;
        private AT.WinForm.ToggleSwitch performanceSwitch;
        private AT.WinForm.ToggleSwitch controllerSwitch;
        private AT.WinForm.ToggleSwitch fanSwitch;
        private System.Windows.Forms.Panel listControlPanel;
        #endregion
    }
}

