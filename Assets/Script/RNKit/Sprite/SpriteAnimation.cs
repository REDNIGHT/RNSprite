// Copyright (c) 2012 Xilin Chen (RN)
// Please direct any bugs/comments/suggestions to http://blog.sina.com.cn/u/2840185437

using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Sprite Animation
/// Please add script SpriteXY.cs or SpriteXZ.cs to this GameObject in editor.
/// 
/// 
/// sub image index sequence
/// |-----------|
/// | 6 | 7 | 8 |
/// |-----------|
/// | 3 | 4 | 5 |
/// |-----------|
/// | 0 | 1 | 2 |
/// |-----------|
/// 
/// </summary>
[AddComponentMenu("RN/Sprite/SpriteAnimation")]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    /// <summary>
    /// Sprite's mat
    /// </summary>
    Material material;


    /// <summary>
    /// If set to true, the animation will automatically start playing on awake
    /// </summary>
    public bool playOnAwake = false;
    /// <summary>
    /// Does the GameObject of this Sprite auto destructs?
    /// </summary>
    public bool autoDestroy = false;
    /// <summary>
    /// face to camera
    /// </summary>
    public bool face2Camera = false;

    /// <summary>
    /// Random Angles enum
    /// </summary>
    /// <remarks></remarks>
    public enum RandomAngles
    {
        None,

        LocalEulerAnglesX,
        LocalEulerAnglesY,
        LocalEulerAnglesZ,

        EulerAnglesX,
        EulerAnglesY,
        EulerAnglesZ,

        TransformForward,
        TransformUp,
        TransformRight,

        AnglesXYZ,
    }
    /// <summary>
    /// Random Angles on start
    /// </summary>
    public RandomAngles randomAngles = RandomAngles.None;

    /// <summary>
    /// Number of frames located across the X axis.
    /// </summary>
    public int xCount = 4;
    /// <summary>
    /// Number of frames located across the Y axis.
    /// </summary>
    public int yCount = 4;

    /// <summary>
    /// Animation Info
    /// </summary>
    /// <remarks></remarks>
    [System.Serializable]
    public class AnimationInfo
    {
        /// <summary>
        /// begin frames index
        /// </summary>
        public int beginIndex;
        /// <summary>
        /// end frames index
        /// </summary>
        public int endIndex;
        /// <summary>
        /// frame time
        /// </summary>
        public float frameTime = 0.1f;
        /// <summary>
        /// is loop
        /// </summary>
        public bool isloop = false;
    }

    /// <summary>
    /// animations list
    /// </summary>
    public List<AnimationInfo> animations;


    /// <summary>
    /// init this sprite.
    /// </summary>
    /// <remarks></remarks>
    void Awake()
    {
        //
        if (Application.isPlaying)
        {
            material = this.renderer.material;

            if (material == null)
                Debug.LogError("material == null");

            if (animations.Count == 0)
                Debug.LogError("animations.Count == 0");
        }
        else
        {
            material = this.renderer.sharedMaterial;
        }


        //
        updateUVScale();
        if (animations != null)
            setSubImage(animations[animationIndex].beginIndex);


        //
        if (face2Camera)
            transform.forward = Camera.main.transform.forward;


        switch (randomAngles)
        {
            case RandomAngles.None:
                break;

            case RandomAngles.LocalEulerAnglesX:
                transform.localEulerAngles = new Vector3(Random.Range(0, 360), 0, 0);
                break;
            case RandomAngles.LocalEulerAnglesY:
                transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                break;
            case RandomAngles.LocalEulerAnglesZ:
                transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                break;


            case RandomAngles.EulerAnglesX:
                transform.eulerAngles = new Vector3(Random.Range(0, 360), 0, 0);
                break;
            case RandomAngles.EulerAnglesY:
                transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                break;
            case RandomAngles.EulerAnglesZ:
                transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                break;


            case RandomAngles.AnglesXYZ:
                transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                break;


            case RandomAngles.TransformForward:
                transform.RotateAround(transform.forward, Random.Range(0, 360));
                break;
            case RandomAngles.TransformUp:
                transform.RotateAround(transform.up, Random.Range(0, 360));
                break;
            case RandomAngles.TransformRight:
                transform.RotateAround(transform.right, Random.Range(0, 360));
                break;
            default:
                throw new System.Exception();
        }
    }

    /// <summary>
    /// Starts this sprite.
    /// </summary>
    /// <remarks></remarks>
    void Start()
    {
        if (playOnAwake)
            play();
    }

    /// <summary>
    /// Updates the UV scale.
    /// </summary>
    /// <remarks></remarks>
    public void updateUVScale()
    {
        if (material != null)
            material.mainTextureScale = new Vector2(1f / xCount, 1f / yCount);
    }

    /// <summary>
    /// Updates this sprite.
    /// </summary>
    /// <remarks></remarks>
    void Update()
    {
        if (_curAnimationInfo != null)
        {
            _curFrameTime -= Time.deltaTime;
            if (_curFrameTime <= 0.0f)
            {
                _curFrameTime += _curAnimationInfo.frameTime;
                ++_subImageIndex;

                if (_subImageIndex > _curAnimationInfo.endIndex)
                {
                    if (_curAnimationInfo.isloop)
                    {
                        _subImageIndex = _curAnimationInfo.beginIndex;
                    }
                    else
                    {
                        //
                        if (onComplete != null)
                            onComplete(this);

                        //
                        _curAnimationInfo = null;

                        //
                        if (autoDestroy)
                            Destroy(gameObject);

                        return;
                    }
                }

                //
                updateSubImage();

            }
        }
    }



    /// <summary>
    /// Currently playing animation information.
    /// Internal use only.
    /// </summary>
    [System.NonSerialized]
    public AnimationInfo _curAnimationInfo;

    /// <summary>
    /// Start playing the animation index.
    /// </summary>
    public int animationIndex;

    /// <summary>
    /// Currently frame time.
    /// </summary>
    float _curFrameTime;

    /// <summary>
    /// Currently sub image index.
    /// </summary>
    int _subImageIndex;

    /// <summary>
    /// Callback type.
    /// </summary>
    /// <param name="a">this SpriteAnimation ins.</param>
    /// <remarks></remarks>
    public delegate void Function(SpriteAnimation a);

    /// <summary>
    /// Call this Function on complete.
    /// </summary>
    public Function onComplete = null;

    /// <summary>
    /// Plays this sprite animation.
    /// </summary>
    public void play()
    {
        _play(animations[animationIndex]);
    }

    /// <summary>
    /// Plays the sprite animation by specified index.
    /// </summary>
    /// <param name="index">animation index.</param>
    public void play(int index)
    {
        animationIndex = index;
        _play(animations[animationIndex]);
    }

    /// <summary>
    /// _plays the specified anim.
    /// Internal use only
    /// </summary>
    /// <param name="anim">The anim.</param>
    public void _play(AnimationInfo anim)
    {
        _curAnimationInfo = anim;
        _curFrameTime = _curAnimationInfo.frameTime;
        _subImageIndex = _curAnimationInfo.beginIndex;
    }

    /// <summary>
    /// Updates the sub image.
    /// </summary>
    /// <remarks></remarks>
    void updateSubImage()
    {
        var xIndex = _subImageIndex % xCount;
        var yIndex = _subImageIndex / yCount;

        var s = material.mainTextureScale;
        material.mainTextureOffset = new Vector2(s.x * xIndex, s.y * yIndex);
    }

    /// <summary>
    /// Sets the sub image index.
    /// </summary>
    /// <param name="_subImageIndex_">The sub image index.</param>
    public void setSubImage(int _subImageIndex_)
    {
        if (material == null)
            material = renderer.sharedMaterial;

        _subImageIndex = _subImageIndex_;
        updateSubImage();
    }

}