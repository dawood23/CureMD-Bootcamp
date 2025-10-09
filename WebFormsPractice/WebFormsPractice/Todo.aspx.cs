using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormsPractice
{
    public partial class Todo : System.Web.UI.Page
    {
/*        protected void Page_Init(object sender, EventArgs e)
        {
            if (ViewState["Todos"] != null)
            {
                var todos = (List<string>)ViewState["Todos"];
                RenderTodos(todos);
            }
        }*/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["Todos"] != null)
            {
                var todos = (List<string>)ViewState["Todos"];
                RenderTodos(todos);
            }
        }

        protected void AddTodo(object sender, EventArgs e)
        {
            string todoValue = TodoText.Text.Trim();
            if (string.IsNullOrEmpty(todoValue)) return;

            var todos = ViewState["Todos"] as List<string> ?? new List<string>();
            todos.Insert(0, todoValue);
            ViewState["Todos"] = todos;

            RenderTodos(todos);
            TodoText.Text = "";
        }

        protected void DeleteTodo(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int index = int.Parse(btn.CommandArgument);

            var todos = (List<string>)ViewState["Todos"];
            todos.RemoveAt(index);
            ViewState["Todos"] = todos;

            RenderTodos(todos);
        }

        private void RenderTodos(List<string> todos)
        {
            TodoContainer.Controls.Clear();

            for (int i = 0; i < todos.Count; i++)
            {
                var todoDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                todoDiv.Attributes["class"] = "todo-item";

                var text = new Label();
                text.Text = todos[i];

                var deleteBtn = new Button();
                deleteBtn.Text = "X";
                deleteBtn.CssClass = "delete-btn";
                deleteBtn.CommandArgument = i.ToString();
                deleteBtn.Click += DeleteTodo;

                todoDiv.Controls.Add(text);
                todoDiv.Controls.Add(deleteBtn);

                TodoContainer.Controls.Add(todoDiv);
            }
        }
    }
}
