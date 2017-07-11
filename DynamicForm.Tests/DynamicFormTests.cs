using DynamicForm.Core;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DynamicForm.Tests
{
    [TestFixture]
    public class DynamicFormTests
    {
        [Test]
        public void CreateAnswers()
        {
            Form f = new Form();
            f.Title.Should().BeNull();
            f.Title = "jj";
            f.Title.Should().Be("jj");

            FormAnswer a = f.FindOrCreateAnswer("Emilie");
            a.Should().NotBeNull();
            FormAnswer b = f.FindOrCreateAnswer("Emilie");
            a.Should().Be(b);
            f.AnswerCount.Should().Be(1);

            FormAnswer c = f.FindOrCreateAnswer("John Doe");
            a.Should().NotBe(c);

            a.UniqueName.Should().Be("Emilie");
            c.UniqueName.Should().Be("John Doe");
        }

        [Test]
        public void CreateQuestionFolders()
        {
            Form f = new Form();
            f.Questions.Title = "oui";
            f.Questions.Title.Should().Be(f.Title);
            QuestionBase q1 = f.Questions.AddNewQuestion("DynamicForm.Tests.CompositeQuestion, DynamicForm.Tests");
            QuestionBase q2 = f.Questions.AddNewQuestion(typeof(CompositeQuestion));
            Assert.AreEqual(0, q1.Index);
            Assert.AreEqual(1, q2.Index);
            q2.SetIndex(0);
            Assert.AreEqual(0, q2.Index);
            Assert.AreEqual(1, q1.Index);
            q2.Parent = null;
            Assert.AreEqual(0, q1.Index);
            q2.Parent = q1;
            Assert.IsTrue(f.Questions.Contains(q1));
            Assert.IsTrue(f.Questions.Contains(q2));
        }

        [Test]
        public void Create_Question_And_Answer_It()
        {
            Form f = new Form();

            OpenQuestion qOpen = (OpenQuestion)f.Questions.AddNewQuestion(typeof(OpenQuestion));
            qOpen.Title = "First Question in the World!";
            qOpen.AllowEmptyAnswer = false;
            qOpen.Title.Should().Be("First Question in the World!");
            qOpen.AllowEmptyAnswer.Should().BeFalse();

            FormAnswer fa = f.CreateAnswer("Emilie");
            AnswerBase theAnswerOfEmilieToOpen = fa.FindAnswer(qOpen);
            if (theAnswerOfEmilieToOpen == null)
            {
                theAnswerOfEmilieToOpen = fa.AddAnswerFor(qOpen);
            }
            theAnswerOfEmilieToOpen.Should().BeOfType(typeof(OpenAnswer));

            OpenAnswer emilieAnswer = (OpenAnswer)theAnswerOfEmilieToOpen;
            emilieAnswer.FreeAnswer = "I'm very happy to be here.";
        }

        [Test]
        public void Set_index()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                list.Add(i);
            }
            SetIndex(list, 0, 5);
            SetIndex(list, 5, 0);

        }

        private void SetIndex(List<int> array, int initial, int index)
        {
            if (index < 0 || index > array.Count) throw new IndexOutOfRangeException();

            int initialIndex = initial;
            array.Insert(index, array[initial]);
            if (index < initialIndex)
                array.RemoveAt(initialIndex + 1);
            else
                array.RemoveAt(index);
        }
    }
}
