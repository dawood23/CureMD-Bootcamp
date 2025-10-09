using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormsPractice
{
    public partial class Counter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Increment_Counter(object sender, EventArgs e)
        {
            int counter=Int32.Parse(counterLabel.Text);

            counter++;

            counterLabel.Text = counter.ToString();
        }
        protected void Decrement_Counter(object sender, EventArgs e)
        {
            int counter=Int32.Parse(counterLabel.Text);
            counter--;
            counterLabel.Text = counter.ToString(); 
        }
    }
}