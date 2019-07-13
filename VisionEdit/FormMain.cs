﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionEdit.FormLib;
using WeifenLuo.WinFormsUI.Docking;

namespace VisionEdit
{
    public partial class FormMain : Form
    {
        #region 变量定义
        private string m_DockPath { get; set; } = string.Empty;
        public FormImageWindow myFormImageWindow = new FormImageWindow();
        public FormJobManage myFormJobManage = new FormJobManage();
        public FormToolBox myFormToolBox = new FormToolBox();
        #endregion

        public FormMain()
        {
            InitializeComponent();
            m_DockPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            InitDockPanel();
        }
        /// <summary>
        /// 窗体对象实例
        /// </summary>
        private static FormMain _instance;
        public static FormMain Instance
        {
            get
            {
                lock(_instance)
                {
                    if (_instance == null)
                        _instance = new FormMain();
                    return _instance;
                }
            }
        }


        private void FormMain_Load(object sender, EventArgs e)
        {
            // 窗体加载到主窗体
            myFormToolBox.Show(this.dockPanel1, DockState.DockLeft);
            myFormJobManage.Show(this.dockPanel1, DockState.DockRight);
            myFormImageWindow.Show(this.dockPanel1, DockState.Document);
        }

        #region 按照配置文件初始化Dockpanel
        private void InitDockPanel()
        {
            try
            {
                //根据配置文件动态加载浮动窗体
                this.dockPanel1.LoadFromXml(this.m_DockPath, delegate (string persistString)
                {
                    //功能窗体
                    if (persistString == typeof(FormToolBox).ToString())
                    {
                        return myFormToolBox;
                    }
                    if (persistString == typeof(FormJobManage).ToString())
                    {
                        return myFormJobManage;
                    }
                    if (persistString == typeof(FormImageWindow).ToString())
                    {
                        return myFormImageWindow;
                    }
                    //主框架之外的窗体不显示
                    return null;
                });
            }
            catch (Exception)
            {
                //配置文件不存在或配置文件有问题时 按系统默认规则加载子窗体
                myFormToolBox.Show(this.dockPanel1, DockState.DockLeft);
                myFormJobManage.Show(this.dockPanel1, DockState.DockRight);
                myFormImageWindow.Show(this.dockPanel1, DockState.Document);
            }
        }
        #endregion


        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(File.Exists(m_DockPath))
            {
                dockPanel1.SaveAsXml(this.m_DockPath);
            }
            
        }
    }
}
