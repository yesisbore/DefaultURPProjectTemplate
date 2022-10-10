using UnityEngine;

namespace UnityCore
{
    namespace ObjectSelect
    {
        public enum ObjectSelectorState { FindControlTarget, WaitForMenuSelect, Editing }
        public enum ObjectEditState { None, Move, Rotate, Scale }

        // Floating Pop Up
        public enum FloatingPopUpMenu { Edit }
        public enum UIPositionType
        {
            UpperLeft, UpperCenter, UpperRight,
            MiddleLeft, MiddleCenter, MiddleRight,
            LowerLeft, LowerCenter, LowerRight,
        }

        public class MovableScreen
        {
            public float Left;
            public float Top;
            public float Right;
            public float Bottom;

            public MovableScreen(Vector2 uiSize)
            {
                Left = uiSize.x * 0.5f;
                Top = uiSize.y * 0.5f;
                Right = Screen.width - uiSize.x;
                Bottom = Screen.height - uiSize.y;
            } // Constructor
        }
        
        public class FloatingPopUpUIPosition
        {
            private Transform _targetObject;
            private Bounds _targetBounds;
            private Vector3[] _positions = new Vector3[9] ;

            private void Update()
            {
                UpdatePositions();
            }
            
            public FloatingPopUpUIPosition(Transform targetObject)
            {
                _targetObject = targetObject;
                _targetBounds = _targetObject.GetComponent<MeshFilter>().mesh.bounds;
            } // Constructor

            public Vector3 GetPosition(UIPositionType uiPositionType)
            {
                UpdatePositions();
                return _positions[(int)uiPositionType];
            } // End of GetPosition

            public void UpdateTargetObject(Transform targetObject)
            {
                _targetObject = targetObject;
            } // End of UpdateTargetObject
            
            private void UpdatePositions()
            {
                if(!_targetObject) return;
                
                var targetPosition = _targetObject.position;
                var right = (_targetBounds.extents.x * _targetObject.localScale.x) * Camera.main.transform.right;
                var top = (_targetBounds.extents.y * _targetObject.localScale.y) * Vector3.up;
                var left = -1.0f * right;
                var bottom = -1.0f * top; 
                
                // Upper
                var topPos      = targetPosition + top;
                var upperLeft   = topPos + left;
                var upperCenter = topPos ;
                var upperRight  = topPos + right;
                
                // Middle
                var middleLeft   = targetPosition + left;
                var middleCenter = targetPosition ;
                var middleRight  = targetPosition + right ;
                
                // Lower
                var bottomPos   = targetPosition + bottom;
                var lowerLeft   = bottomPos + left;
                var lowerCenter = bottomPos ;
                var lowerRight  = bottomPos + right;

                _positions[0] = upperLeft;
                _positions[1] = upperCenter;
                _positions[2] = upperRight;
                _positions[3] = middleLeft;
                _positions[4] = middleCenter;
                _positions[5] = middleRight;
                _positions[6] = lowerLeft;
                _positions[7] = lowerCenter;
                _positions[8] = lowerRight;
            } // End of UpdatePositions
        }

        
        // Indicator
        public enum IndicatorType{ None, Circle, Box, Arrow }
        public enum IndicatorPosition{ Top, Center, Bottom, Forward }
    }
}