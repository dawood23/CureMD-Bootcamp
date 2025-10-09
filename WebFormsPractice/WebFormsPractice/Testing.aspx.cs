using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormsPractice
{
    public partial class Testing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            string Name = Nametxt.Text;
            string Email=Emailtxt.Text;

            NameOutput.Text ="Name: " +Name;
            EmailOutput.Text ="Email: "+ Email;
        }
    }
}