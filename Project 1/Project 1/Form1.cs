using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Collections;
using System.Text.RegularExpressions;

namespace Project_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // lay danh sach card mang dua vao combobox
        private void goidanhsachcard()
        {
            ArrayList tencard = WMI.laydanhsachcar();
            cbocard.Items.Clear();
            foreach (string ten in tencard)
            {
                cbocard.Items.Add(ten);
            }
            if (cbocard.Items.Count > 0)
                cbocard.SelectedIndex = 0;
        }
        

        //lay thong tin card de hien thi

        private void Getnicinfo(string tencard)
        {
            string[] ipaddress, subnet, gateway, dns;
            WMI.GetNicInfo(tencard, out ipaddress, out subnet, out gateway, out dns);
            if (ipaddress == null || subnet == null)
                return;
            lblIP.Text = null;
            lblIP.Text = string.Join(",", ipaddress);
            lblSubnet.Text = null;
            lblSubnet.Text = string.Join(",", subnet);
            if (gateway != null)
            {
                lblGateway.Text = null;
                lblGateway.Text = string.Join(",", gateway);
            }
            else
                lblGateway.Text = null;
            if (dns != null)
            {
                lblDNS.Text = null;
                lblDNS.Text = string.Join(",", dns);
            }
            else 
                lblDNS.Text = null ;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            goidanhsachcard();
            txtip.Enabled = false;
            txtsubnet.Enabled = false;
            txtgateway.Enabled = false;
            txtdns.Enabled = false;
        }

        // cap nhat khi chon card
        private void cbocard_SelectedIndexChanged(object sender, EventArgs e)
        {
            Getnicinfo( cbocard.SelectedItem.ToString());
        }


        // thay doi lua chon o checbox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.Text = " cap nhat ip (uncheck) ";
                txtip.Text = null;
                txtsubnet.Text = null;
                txtgateway.Text = null;
                txtdns.Text = null;
                txtip.Enabled = true;
                txtsubnet.Enabled = true;
                txtgateway.Enabled = true;
                txtdns.Enabled = true;
            }
            else
            {
                checkBox1.Text = " Use (check) ";
                txtip.Text = null;
                txtip.Enabled = false;
                txtsubnet.Text = null;
                txtsubnet.Enabled = false;
                txtgateway.Text = null;
                txtgateway.Enabled = false;
                txtdns.Text = null;
                txtdns.Enabled = false;
            }
        }
        // nut apply
        private void btnapply_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (!string.IsNullOrEmpty(txtip.Text) && !string.IsNullOrEmpty(txtsubnet.Text))
                {
                    if (checkIP(txtip.Text) == false)
                    {
                        MessageBox.Show("Invalid IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtip.Focus();
                    }
                    else
                    {
                        if (checkIP(txtsubnet.Text) == false)
                        {
                            MessageBox.Show("Invalid Subnet Mask", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtsubnet.Focus();
                        }
                        else
                        {
                            WMI.SetIP(cbocard.SelectedItem.ToString(), txtip.Text, txtsubnet.Text, txtgateway.Text, txtdns.Text);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtip.Text))
                    {
                        MessageBox.Show("Ip Address is null", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtip.Focus();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtsubnet.Text))
                        {
                            MessageBox.Show("Subnet mask is null", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtsubnet.Focus();
                        }
                    }
                }

            }
            if (checkBox1.Checked == false)
            {
                WMI.SetDHCP(cbocard.SelectedItem.ToString());
            }
            Getnicinfo(cbocard.SelectedItem.ToString());
        }

        //Kiem tra ip hop le
        private bool checkIP(string ipaddress)
        {
            Regex myRegex = new Regex("^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            if (myRegex.IsMatch(ipaddress))
            {
                return true;
            }
            return false;
        }

        private void txtgateway_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtgateway.Text))
            {
                if (checkIP(txtgateway.Text) == false)
                {
                    MessageBox.Show("Invalid Default Gateway Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtgateway.Focus();
                }
            }
        }

        private void txtdns_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtdns.Text))
            {
                if (checkIP(txtdns.Text) == false)
                {
                    MessageBox.Show("Invalid Dns Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtdns.Focus();
                }
            }
        }


       
    }
}
