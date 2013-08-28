namespace OMR.Core.Helpers.Patterns
{
    using System;
    using System.Collections.Generic;

    public interface IMomentoOriginator<T>
    {
        T CreateMomento();

        void RestoreMomento(T state);
    }

    public class MomentoCaretaker<T> where T : IMomentoOriginator<T>
    {
        private int _currentIndex;
        private int _capacity;

        private IList<T> _undoRedoList;

        public IMomentoOriginator<T> _momentableObject { get; set; }

        /// <summary>
        /// Count of undo redo history
        /// </summary>
        /// <param name="capacity"></param>
        public MomentoCaretaker(IMomentoOriginator<T> momentableObject, int capacity = 10)
        {
            if (momentableObject == null)
                throw new ArgumentException("momentableObject");

            if (capacity < 1)
                throw new ArgumentException("Capacity not less than one");

            _currentIndex = 0;
            _undoRedoList = new List<T>();

            _momentableObject = momentableObject;
            _capacity = capacity;
        }

        /// <summary>
        /// Must be called when momentable object changed.
        /// </summary>
        public void Step()
        {
            // Delete the old redo step(s) when new step fired
            while (IsRedoEnabled())
            {
                _undoRedoList.RemoveAt(_undoRedoList.Count - 1);
            }

            // Get the current state from momentable object
            var currentState = _momentableObject.CreateMomento();

            // Add to undo redo list
            _undoRedoList.Add(currentState);

            // Remove head object when capacity exceededi
            if (_undoRedoList.Count > _capacity)
            {
                _undoRedoList.RemoveAt(0);
            }

            ++_currentIndex;
        }

        public void Undo()
        {
            if (!IsUndoEnabled())
                throw new InvalidOperationException("Undo is not enabled");

            _currentIndex -= 1;

            var momento = _undoRedoList[_currentIndex]; // check null or overhead?

            _momentableObject.RestoreMomento(momento);
        }

        public void Redo()
        {
            if (!IsRedoEnabled())
                throw new InvalidOperationException("Redo is not enabled");

            _currentIndex += 1;

            var momento = _undoRedoList[_currentIndex]; // check null or overhead?

            _momentableObject.RestoreMomento(momento);
        }

        public bool IsUndoEnabled()
        {
            return (_currentIndex > 0 && _undoRedoList.Count > 0);
        }

        private bool IsRedoEnabled()
        {
            return (_undoRedoList.Count > 0 && (_currentIndex - 1 < _undoRedoList.Count));
        }

    }

}