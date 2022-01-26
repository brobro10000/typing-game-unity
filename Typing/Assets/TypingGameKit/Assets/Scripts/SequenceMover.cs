using System.Collections.Generic;
using UnityEngine;

namespace TypingGameKit
{
    /// <summary>
    /// Sequence mover updates the position of a sequence.
    /// </summary>
    public class SequenceMover
    {
        private Vector2 avoidanceVelocity;
        private Vector3[] cornerArray = new Vector3[4];
        private Vector2 overlapAvoidanceOffset;
        private ManagedSequence sequence;
        private SequenceSettings settings;
        private Camera camera;
        private Vector3[] parentCorners;
        private Vector2 anchorPosition;

        /// <summary>
        /// Instantiating a new sequence mover.
        /// </summary>
        public SequenceMover(ManagedSequence sequence, SequenceSettings settings)
        {
            this.sequence = sequence;
            this.settings = settings;
        }

        /// <summary>
        /// Array containing the corners of the sequence in the canvas.
        /// </summary>
        public Vector3[] CornerArray
        {
            get { return cornerArray; }
        }

        /// <summary>
        /// Sets the position of the sequence to it's default position.
        /// </summary>
        public void InitializePosition(Camera camera)
        {
            this.camera = camera;
            sequence.transform.position = GetAnchorPosition();
        }

        /// <summary>
        /// Should be called before updating a group of sequence movers.
        /// </summary>
        public void PrepareForMovementUpdate(Vector3[] parentCorners, Camera camera)
        {
            this.camera = camera;
            this.parentCorners = parentCorners;
            ((RectTransform)sequence.Display.transform).GetWorldCorners(cornerArray);
            anchorPosition = GetAnchorPosition();
        }

        /// <summary>
        /// Update the position of the associated sequence.
        /// </summary>
        public void UpdateMovement(List<ManagedSequence> sequences)
        {
            Vector2 desiredPosition = anchorPosition;

            if (settings.UseOverlapAvoidance)
            {
                UpdateOverlapAvoidance(sequences, anchorPosition);
                desiredPosition += overlapAvoidanceOffset;
            }

            if (settings.RestrictToParent)
            {
                desiredPosition = GetParentRestrictedPosition(desiredPosition);
            }

            if (settings.UseSoftMovement)
            {
                ApplySoftMovement(desiredPosition);
            }
            else
            {
                sequence.transform.position = desiredPosition;
            }
        }

        private Vector2 ApplySoftMovement(Vector2 desiredPosition)
        {
            // move towards desired position
            Vector2 currentToDesired = desiredPosition - (Vector2)sequence.transform.position;
            float distance = Mathf.Min(settings.SoftMovementSettings.moveSpeed * Time.deltaTime, currentToDesired.magnitude);
            sequence.transform.Translate(currentToDesired.normalized * distance);
            return desiredPosition;
        }

        private static Vector2 GetRandomDirection()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        private static bool Overlaps(Vector3[] corners1, Vector3[] corners2)
        {
            return corners2[2].x >= corners1[0].x &&
                   corners2[0].x <= corners1[2].x &&
                   corners2[2].y >= corners1[0].y &&
                   corners2[0].y <= corners1[2].y;
        }

        private Vector3 GetAnchorPosition()
        {
            Vector3 worldPosition = sequence.WorldAnchor.position;
            Vector3 cameraForward = camera.transform.forward;
            Vector3 cameraPosition = camera.transform.position;
            Vector3 cameraToPositionOffset = worldPosition - cameraPosition;

            float product = Vector3.Dot(cameraForward, cameraToPositionOffset);
            bool isBehindCamera = product <= 0;

            if (isBehindCamera)
            {
                Vector3 forwardPushedPosition = worldPosition - (cameraForward * product * 1.01f);
                return camera.WorldToScreenPoint(forwardPushedPosition);
            }
            else
            {
                return camera.WorldToScreenPoint(worldPosition);
            }
        }

        private Vector2 GetParentRestrictedPosition(Vector2 desiredPosition)
        {
            Vector3 currentPosition = sequence.transform.position;
            Vector3 parentBottomLeft = parentCorners[0];
            Vector3 parentTopRight = parentCorners[2];
            Vector3 topRight = CornerArray[2];
            Vector3 bottomLeft = CornerArray[0];

            float leftOffset = bottomLeft.x - currentPosition.x;
            float rightOffset = topRight.x - currentPosition.x;
            float bottomOffset = bottomLeft.y - currentPosition.y;
            float topOffset = topRight.y - currentPosition.y;

            float x = desiredPosition.x;
            if (parentBottomLeft.x > x + leftOffset)
            {
                x = parentBottomLeft.x - leftOffset;
            }
            else if (parentTopRight.x < x + rightOffset)
            {
                x = parentTopRight.x - rightOffset;
            }

            float y = desiredPosition.y;
            if (parentBottomLeft.y > y + bottomOffset)
            {
                y = parentBottomLeft.y - bottomOffset;
            }
            else if (parentTopRight.y < y + topOffset)
            {
                y = parentTopRight.y - topOffset;
            }

            return new Vector2(x, y);
        }

        private void UpdateOverlapAvoidance(List<ManagedSequence> sequences, Vector2 anchorPosition)
        {
            Vector2 sequencePosition = sequence.transform.position;
            Vector2 escapeOffset = Vector2.zero;

            for (int index = 0; index < sequences.Count; index++)
            {
                ManagedSequence otherSequence = sequences[index];
                if (otherSequence != sequence && Overlaps(CornerArray, otherSequence.Mover.CornerArray))
                {
                    if (Vector2.Distance(sequencePosition, otherSequence.transform.position) < 0.01f)
                    {
                        escapeOffset += GetRandomDirection() * settings.OverlapAvoidanceSetting.overlapReluctance;
                    }
                    else
                    {
                        escapeOffset += (sequencePosition - (Vector2)otherSequence.transform.position) * settings.OverlapAvoidanceSetting.overlapReluctance;
                    }
                }
            }

            Vector2 targetOffset = anchorPosition - sequencePosition;
            if (targetOffset.sqrMagnitude < escapeOffset.sqrMagnitude)
            {
                // increase velocity
                Vector2 velocityIncrease = escapeOffset.normalized * settings.OverlapAvoidanceSetting.avoidanceStrength;
                avoidanceVelocity += velocityIncrease * Time.deltaTime;
            }
            else
            {
                // let velocity fade over time
                avoidanceVelocity -= avoidanceVelocity * Time.deltaTime;
            }

            // let offset fade over time
            overlapAvoidanceOffset -= overlapAvoidanceOffset * Time.deltaTime;

            // update offset position
            overlapAvoidanceOffset += avoidanceVelocity * Time.deltaTime;
        }
    }
}