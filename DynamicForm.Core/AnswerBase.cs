namespace DynamicForm.Core
{
    public abstract class AnswerBase
    {
        public AnswerBase(QuestionBase qBase)
        {
            Question = qBase;
        }

        public QuestionBase Question { get; private set; }
    }
}