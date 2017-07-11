namespace DynamicForm.Core
{
    public class AnswerBase
    {
        public AnswerBase(QuestionBase qBase)
        {
            Question = qBase;
        }

        public QuestionBase Question { get; private set; }
    }
}