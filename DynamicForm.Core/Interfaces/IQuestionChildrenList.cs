using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForm.Core.Interfaces
{
    public interface IQuestionChildrenList : IReadOnlyList<QuestionBase>
    {
        bool Contains(QuestionBase qBase);
    }
}
