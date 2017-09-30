using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicForm.Core.Interfaces;

namespace DynamicForm.Core
{
    internal class QuestionChildrensList : IQuestionChildrenList
    {
        List<QuestionBase> _qBases;
        QuestionBase _qBase;

        public QuestionChildrensList(List<QuestionBase> listQBase, QuestionBase qBase)
        {
            _qBases = listQBase;
            _qBase = qBase;
        }

        public QuestionBase this[int index] => _qBases[index];

        public int Count => _qBases.Count;

        public bool Contains(QuestionBase qBase)
        {
            return qBase.Contains(qBase);
        }

        public IEnumerator<QuestionBase> GetEnumerator()
        {
            return _qBases.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _qBases.GetEnumerator();
        }
    }
}
