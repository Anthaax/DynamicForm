namespace DynamicForm.Core
{
    public class OpenQuestion : QuestionBase
    {
        public OpenQuestion(QuestionBase parent) : base(parent)
        {
        }

        public bool AllowEmptyAnswer { get; set; }
        public override AnswerBase AnswerModel => new OpenAnswer(this);
    }
}