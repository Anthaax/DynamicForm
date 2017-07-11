using DynamicForm.Core;

namespace DynamicForm.Tests
{
    internal class CompositeQuestion : QuestionBase
    {
        public CompositeQuestion(QuestionBase parent) : base( parent)
        {
        }
    }
}