using System;
using DynamicForm.Core;

namespace DynamicForm.Tests
{
    internal class CompositeQuestion : QuestionBase
    {
        public CompositeQuestion(QuestionBase parent) : base( parent)
        {
        }

        public override AnswerBase AnswerModel => throw new NotImplementedException();
    }
}