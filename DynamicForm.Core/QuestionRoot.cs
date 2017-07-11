using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForm.Core
{
    internal class QuestionRoot : QuestionBase
    {
        Form _form;
        public QuestionRoot(Form form) : base(null)
        {
            _form = form;
        }

        public override string Title { get { return _form.Title; } set { _form.Title = value; } }
        public override Form Form => _form;
    }
}
