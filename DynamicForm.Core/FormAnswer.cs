using System;
using System.Collections.Generic;

namespace DynamicForm.Core
{
    public class FormAnswer
    {
        readonly Dictionary<QuestionBase, AnswerBase> _answers;
        public FormAnswer(string name)
        {
            UniqueName = name;
            _answers = new Dictionary<QuestionBase, AnswerBase>();
        }
        public string UniqueName { get; set; }

        public AnswerBase FindAnswer(OpenQuestion qOpen)
        {
            AnswerBase ab;
            _answers.TryGetValue(qOpen, out ab);
            return ab;
        }

        public AnswerBase AddAnswerFor(OpenQuestion qOpen)
        {
            AnswerBase ab = qOpen.AnswerModel;
            _answers.Add(qOpen, ab);
            return ab;
        }
    }
}