namespace DynamicForm.Core
{
    public class OpenAnswer : AnswerBase
    {
        public OpenAnswer(QuestionBase qBase) : base(qBase)
        {
        }

        public string FreeAnswer { get; set; }
    }
}