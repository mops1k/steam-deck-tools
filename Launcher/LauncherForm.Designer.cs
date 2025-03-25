using System.ComponentModel;
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
            powerControl = new System.Windows.Forms.CheckBox();
            steamController = new System.Windows.Forms.CheckBox();
            performanceOverlay = new System.Windows.Forms.CheckBox();
            fanControl = new System.Windows.Forms.CheckBox();
            generateShortcuts = new System.Windows.Forms.Button();
            restartButton = new System.Windows.Forms.Button();
            stopButton = new System.Windows.Forms.Button();
            startButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // powerControl
            // 
            powerControl.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            powerControl.Checked = true;
            powerControl.CheckState = System.Windows.Forms.CheckState.Checked;
            powerControl.Dock = System.Windows.Forms.DockStyle.Top;
            powerControl.FlatAppearance.BorderSize = 0;
            powerControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            powerControl.Font = new System.Drawing.Font("Verdana Pro Cond Semibold", 9F, System.Drawing.FontStyle.Bold);
            powerControl.Image = global::Launcher.Resources.traffic_light_outline;
            powerControl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            powerControl.Location = new System.Drawing.Point(10, steamController.Bottom);
            powerControl.Name = "powerControl";
            powerControl.Padding = new System.Windows.Forms.Padding(10);
            powerControl.Size = new System.Drawing.Size(275, 43);
            powerControl.TabIndex = 8;
            powerControl.Text = "PowerControl";
            powerControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            powerControl.UseVisualStyleBackColor = true;
            // 
            // steamController
            // 
            steamController.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            steamController.Checked = true;
            steamController.CheckState = System.Windows.Forms.CheckState.Checked;
            steamController.Dock = System.Windows.Forms.DockStyle.Top;
            steamController.FlatAppearance.BorderSize = 0;
            steamController.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            steamController.Font = new System.Drawing.Font("Verdana Pro Cond Semibold", 9F, System.Drawing.FontStyle.Bold);
            steamController.Image = global::Launcher.Resources.microsoft_xbox_controller;
            steamController.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            steamController.Location = new System.Drawing.Point(10, performanceOverlay.Bottom);
            steamController.Name = "steamController";
            steamController.Padding = new System.Windows.Forms.Padding(10);
            steamController.Size = new System.Drawing.Size(275, 43);
            steamController.TabIndex = 7;
            steamController.Text = "SteamController";
            steamController.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            steamController.UseVisualStyleBackColor = true;
            // 
            // performanceOverlay
            // 
            performanceOverlay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            performanceOverlay.Checked = true;
            performanceOverlay.CheckState = System.Windows.Forms.CheckState.Checked;
            performanceOverlay.Dock = System.Windows.Forms.DockStyle.Top;
            performanceOverlay.FlatAppearance.BorderSize = 0;
            performanceOverlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            performanceOverlay.Font = new System.Drawing.Font("Verdana Pro Cond Semibold", 9F, System.Drawing.FontStyle.Bold);
            performanceOverlay.Image = global::Launcher.Resources.poll;
            performanceOverlay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            performanceOverlay.Location = new System.Drawing.Point(10, fanControl.Bottom);
            performanceOverlay.Name = "performanceOverlay";
            performanceOverlay.Padding = new System.Windows.Forms.Padding(10);
            performanceOverlay.Size = new System.Drawing.Size(275, 43);
            performanceOverlay.TabIndex = 6;
            performanceOverlay.Text = "PerformanceOverlay";
            performanceOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            performanceOverlay.UseVisualStyleBackColor = true;
            // 
            // fanControl
            // 
            fanControl.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            fanControl.Checked = true;
            fanControl.CheckState = System.Windows.Forms.CheckState.Checked;
            fanControl.Dock = System.Windows.Forms.DockStyle.Top;
            fanControl.FlatAppearance.BorderSize = 0;
            fanControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fanControl.Font = new System.Drawing.Font("Verdana Pro Cond Semibold", 9F, System.Drawing.FontStyle.Bold);
            fanControl.Image = global::Launcher.Resources.fan;
            fanControl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fanControl.Location = new System.Drawing.Point(10, 10);
            fanControl.Name = "fanControl";
            fanControl.Padding = new System.Windows.Forms.Padding(10);
            fanControl.Size = new System.Drawing.Size(275, 43);
            fanControl.TabIndex = 5;
            fanControl.Text = "FanControl";
            fanControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            fanControl.UseVisualStyleBackColor = true;
            // 
            // generateShortcuts
            // 
            generateShortcuts.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)128)), ((int)((byte)0)));
            generateShortcuts.Dock = System.Windows.Forms.DockStyle.Top;
            generateShortcuts.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            generateShortcuts.ForeColor = System.Drawing.SystemColors.ButtonFace;
            generateShortcuts.Location = new System.Drawing.Point(10, restartButton.Bottom);
            generateShortcuts.Name = "generateShortcuts";
            generateShortcuts.Padding = new System.Windows.Forms.Padding(5);
            generateShortcuts.Size = new System.Drawing.Size(275, 80);
            generateShortcuts.TabIndex = 12;
            generateShortcuts.Text = "GENERATE SHORTCUTS TO DESKTOP";
            generateShortcuts.UseVisualStyleBackColor = false;
            generateShortcuts.Click += generateShortcuts_Click;
            // 
            // restartButton
            // 
            restartButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            restartButton.Dock = System.Windows.Forms.DockStyle.Top;
            restartButton.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            restartButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            restartButton.Image = global::Launcher.Resources.refresh;
            restartButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            restartButton.Location = new System.Drawing.Point(10, stopButton.Bottom);
            restartButton.Name = "restartButton";
            restartButton.Padding = new System.Windows.Forms.Padding(5);
            restartButton.Size = new System.Drawing.Size(275, 46);
            restartButton.TabIndex = 11;
            restartButton.Text = "RESTART";
            restartButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            restartButton.UseVisualStyleBackColor = false;
            restartButton.Click += restartButton_Click;
            // 
            // stopButton
            // 
            stopButton.BackColor = System.Drawing.Color.Red;
            stopButton.Dock = System.Windows.Forms.DockStyle.Top;
            stopButton.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            stopButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            stopButton.Image = global::Launcher.Resources.stop_button;
            stopButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            stopButton.Location = new System.Drawing.Point(10, startButton.Bottom);
            stopButton.Name = "stopButton";
            stopButton.Padding = new System.Windows.Forms.Padding(5);
            stopButton.Size = new System.Drawing.Size(275, 46);
            stopButton.TabIndex = 10;
            stopButton.Text = "STOP";
            stopButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            stopButton.UseVisualStyleBackColor = false;
            stopButton.Click += stopButton_Click;
            // 
            // startButton
            // 
            startButton.BackColor = System.Drawing.Color.SeaGreen;
            startButton.Dock = System.Windows.Forms.DockStyle.Top;
            startButton.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            startButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            startButton.Image = global::Launcher.Resources.power_button;
            startButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            startButton.Location = new System.Drawing.Point(10, powerControl.Bottom);
            startButton.Name = "startButton";
            startButton.Padding = new System.Windows.Forms.Padding(5);
            startButton.Size = new System.Drawing.Size(275, 46);
            startButton.TabIndex = 9;
            startButton.Text = "START";
            startButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            startButton.UseVisualStyleBackColor = false;
            startButton.Click += startButton_Click;
            // 
            // LauncherForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(295, 402);
            Controls.Add(generateShortcuts);
            Controls.Add(restartButton);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Controls.Add(powerControl);
            Controls.Add(steamController);
            Controls.Add(performanceOverlay);
            Controls.Add(fanControl);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(313, 450);
            MinimumSize = new System.Drawing.Size(313, 450);
            Padding = new System.Windows.Forms.Padding(10);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Manage SteamDeck Tools";
            ResumeLayout(false);
        }
        private System.Windows.Forms.CheckBox powerControl;
        private System.Windows.Forms.CheckBox steamController;
        private System.Windows.Forms.CheckBox performanceOverlay;
        private System.Windows.Forms.CheckBox fanControl;
        private System.Windows.Forms.Button generateShortcuts;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        #endregion
    }
}

