﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Runtime.InteropServices;

namespace OCRGet
{

    public partial class FormDraw : Form
    {
        //[DllImport("User32.dll")]
        //public static extern IntPtr GetDC(IntPtr hwnd);
        //[DllImport("User32.dll")]
        //public static extern void ReleaseDC(IntPtr dc);

        private Rectangle rectRegion;
        private Point startPoint, endPoint;
        private Boolean isDrawing = false;

        public Rectangle area { get { return this.rectRegion; } set { this.rectRegion = value; } }

        public FormDraw()
        {
            InitializeComponent();

            ShowInTaskbar = false;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Bounds = Screen.FromPoint(Cursor.Position).Bounds;
            TopMost = true;
            Opacity = .004;
        }

        private void FormDraw_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;

            Point mousePos = DPIUtil.CorrectMousePos(sender as Control, e.X, e.Y);
            startPoint.X = endPoint.X = mousePos.X;
            startPoint.Y = endPoint.Y = mousePos.Y;
            rectRegion.X = startPoint.X;
            rectRegion.Y = startPoint.Y;
            rectRegion.Width = 0;
            rectRegion.Height = 0;
        }

        private void FormDraw_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            ControlPaint.DrawReversibleFrame(rectRegion, this.BackColor, FrameStyle.Dashed);
            Hide();
        }

        private void FormDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            ControlPaint.DrawReversibleFrame(rectRegion, this.BackColor, FrameStyle.Dashed);

            Point mousePos = DPIUtil.CorrectMousePos(sender as Control, e.X, e.Y);
            endPoint.X = mousePos.X;
            endPoint.Y = mousePos.Y;

            rectRegion.X = Math.Min(startPoint.X, endPoint.X);
            rectRegion.Y = Math.Min(startPoint.Y, endPoint.Y);
            rectRegion.Width = Math.Abs(endPoint.X - startPoint.X);
            rectRegion.Height = Math.Abs(endPoint.Y - startPoint.Y);

            ControlPaint.DrawReversibleFrame(rectRegion, this.BackColor, FrameStyle.Dashed);

            //IntPtr desktopDC = GetDC(IntPtr.Zero);
            //Graphics g = Graphics.FromHdc(desktopDC);
            //g.DrawRectangle(System.Drawing.Pens.Red, rectDraw);
            //g.Dispose();
            //ReleaseDC(desktopDC);
        }

        private void FormDraw_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                if (isDrawing)
                    ControlPaint.DrawReversibleFrame(rectRegion, this.BackColor, FrameStyle.Dashed);
                isDrawing = false;
                rectRegion.Width = 0;
                Hide();
            }
        }

        private void FormDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

    }


}
