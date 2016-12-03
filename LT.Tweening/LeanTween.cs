using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LT.Tweening
{
	public class LeanTween : MonoBehaviour
	{
		public static bool throwErrors = true;

		public static float tau = 6.28318548f;

		private static LTDescr[] tweens;

		private static int[] tweensFinished;

		private static LTDescr tween;

		private static int tweenMaxSearch = -1;

		private static int maxTweens = 400;

		private static int frameRendered = -1;

		private static GameObject _tweenEmpty;

		private static float dtEstimated = -1f;

		public static float dtManual;

		private static float dt;

		private static float dtActual;

		private static int i;

		private static int j;

		private static int finishedCnt;

		private static AnimationCurve punch = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.112586f, 0.9976035f),
			new Keyframe(0.3120486f, -0.1720615f),
			new Keyframe(0.4316337f, 0.07030682f),
			new Keyframe(0.5524869f, -0.03141804f),
			new Keyframe(0.6549395f, 0.003909959f),
			new Keyframe(0.770987f, -0.009817753f),
			new Keyframe(0.8838775f, 0.001939224f),
			new Keyframe(1f, 0f)
		});

		private static AnimationCurve shake = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.25f, 1f),
			new Keyframe(0.75f, -1f),
			new Keyframe(1f, 0f)
		});

		private static Transform trans;

		private static float timeTotal;

		private static TweenAction tweenAction;

		private static float ratioPassed;

		private static float from;

		private static float val;

		private static bool isTweenFinished;

		private static int maxTweenReached;

		private static Vector3 newVect;

		private static GameObject target;

		private static GameObject customTarget;

		public static int startSearch = 0;

		public static LTDescr d;

		private static Action<LTEvent>[] eventListeners;

		private static GameObject[] goListeners;

		private static int eventsMaxSearch = 0;

		public static int EVENTS_MAX = 10;

		public static int LISTENERS_MAX = 10;

		private static int INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;

		public static int maxSearch
		{
			get
			{
				return LeanTween.tweenMaxSearch;
			}
		}

		public static int tweensRunning
		{
			get
			{
				int num = 0;
				for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
				{
					if (LeanTween.tweens[i].toggle)
					{
						num++;
					}
				}
				return num;
			}
		}

		public static GameObject tweenEmpty
		{
			get
			{
				LeanTween.init(LeanTween.maxTweens);
				return LeanTween._tweenEmpty;
			}
		}

		public static void init()
		{
			LeanTween.init(LeanTween.maxTweens);
		}

		public static void init(int maxSimultaneousTweens)
		{
			if (LeanTween.tweens == null)
			{
				LeanTween.maxTweens = maxSimultaneousTweens;
				LeanTween.tweens = new LTDescr[LeanTween.maxTweens];
				LeanTween.tweensFinished = new int[LeanTween.maxTweens];
				LeanTween._tweenEmpty = new GameObject();
				LeanTween._tweenEmpty.set_name("~LeanTween");
				LeanTween._tweenEmpty.AddComponent(typeof(LeanTween));
				LeanTween._tweenEmpty.set_isStatic(true);
				LeanTween._tweenEmpty.set_hideFlags(61);
				Object.DontDestroyOnLoad(LeanTween._tweenEmpty);
				for (int i = 0; i < LeanTween.maxTweens; i++)
				{
					LeanTween.tweens[i] = new LTDescr();
				}
			}
		}

		public static void reset()
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				LeanTween.tweens[i].toggle = false;
			}
			LeanTween.tweens = null;
			Object.Destroy(LeanTween._tweenEmpty);
		}

		public void Update()
		{
			LeanTween.update();
		}

		public void OnLevelWasLoaded(int lvl)
		{
			LTGUI.reset();
		}

		public static void update()
		{
			if (LeanTween.frameRendered != Time.get_frameCount())
			{
				LeanTween.init();
				if (LeanTween.dtEstimated < 0f)
				{
					LeanTween.dtEstimated = 0f;
				}
				else
				{
					LeanTween.dtEstimated = Time.get_unscaledDeltaTime();
				}
				LeanTween.dtActual = Time.get_deltaTime();
				LeanTween.maxTweenReached = 0;
				LeanTween.finishedCnt = 0;
				int num = 0;
				while (num <= LeanTween.tweenMaxSearch && num < LeanTween.maxTweens)
				{
					if (LeanTween.tweens[num].toggle)
					{
						LeanTween.maxTweenReached = num;
						LeanTween.tween = LeanTween.tweens[num];
						LeanTween.trans = LeanTween.tween.trans;
						LeanTween.timeTotal = LeanTween.tween.time;
						LeanTween.tweenAction = LeanTween.tween.type;
						if (LeanTween.tween.useEstimatedTime)
						{
							LeanTween.dt = LeanTween.dtEstimated;
						}
						else if (LeanTween.tween.useFrames)
						{
							LeanTween.dt = 1f;
						}
						else if (LeanTween.tween.useManualTime)
						{
							LeanTween.dt = LeanTween.dtManual;
						}
						else if (LeanTween.tween.direction == 0f)
						{
							LeanTween.dt = 0f;
						}
						else
						{
							LeanTween.dt = LeanTween.dtActual;
						}
						if (LeanTween.trans == null)
						{
							LeanTween.removeTween(num);
						}
						else
						{
							LeanTween.isTweenFinished = false;
							if (LeanTween.tween.delay <= 0f)
							{
								if (LeanTween.tween.passed + LeanTween.dt > LeanTween.tween.time && LeanTween.tween.direction > 0f)
								{
									LeanTween.isTweenFinished = true;
									LeanTween.tween.passed = LeanTween.tween.time;
								}
								else if (LeanTween.tween.direction < 0f && LeanTween.tween.passed - LeanTween.dt < 0f)
								{
									LeanTween.isTweenFinished = true;
									LeanTween.tween.passed = Mathf.Epsilon;
								}
							}
							if (!LeanTween.tween.hasInitiliazed && (((double)LeanTween.tween.passed == 0.0 && (double)LeanTween.tween.delay == 0.0) || (double)LeanTween.tween.passed > 0.0))
							{
								LeanTween.tween.init();
							}
							if (LeanTween.tween.delay <= 0f)
							{
								if (LeanTween.timeTotal <= 0f)
								{
									LeanTween.ratioPassed = 1f;
								}
								else
								{
									LeanTween.ratioPassed = LeanTween.tween.passed / LeanTween.timeTotal;
								}
								if (LeanTween.ratioPassed > 1f)
								{
									LeanTween.ratioPassed = 1f;
								}
								else if (LeanTween.ratioPassed < 0f)
								{
									LeanTween.ratioPassed = 0f;
								}
								if (LeanTween.tweenAction >= TweenAction.MOVE_X && LeanTween.tweenAction < TweenAction.MOVE)
								{
									if (LeanTween.tween.animationCurve != null)
									{
										LeanTween.val = LeanTween.tweenOnCurve(LeanTween.tween, LeanTween.ratioPassed);
									}
									else
									{
										switch (LeanTween.tween.tweenType)
										{
										case LeanTweenType.linear:
											LeanTween.val = LeanTween.tween.from.x + LeanTween.tween.diff.x * LeanTween.ratioPassed;
											break;
										case LeanTweenType.easeOutQuad:
											LeanTween.val = LeanTween.easeOutQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInQuad:
											LeanTween.val = LeanTween.easeInQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInOutQuad:
											LeanTween.val = LeanTween.easeInOutQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInCubic:
											LeanTween.val = LeanTween.easeInCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeOutCubic:
											LeanTween.val = LeanTween.easeOutCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInOutCubic:
											LeanTween.val = LeanTween.easeInOutCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInQuart:
											LeanTween.val = LeanTween.easeInQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeOutQuart:
											LeanTween.val = LeanTween.easeOutQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInOutQuart:
											LeanTween.val = LeanTween.easeInOutQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInQuint:
											LeanTween.val = LeanTween.easeInQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeOutQuint:
											LeanTween.val = LeanTween.easeOutQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInOutQuint:
											LeanTween.val = LeanTween.easeInOutQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInSine:
											LeanTween.val = LeanTween.easeInSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeOutSine:
											LeanTween.val = LeanTween.easeOutSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInOutSine:
											LeanTween.val = LeanTween.easeInOutSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInExpo:
											LeanTween.val = LeanTween.easeInExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeOutExpo:
											LeanTween.val = LeanTween.easeOutExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInOutExpo:
											LeanTween.val = LeanTween.easeInOutExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInCirc:
											LeanTween.val = LeanTween.easeInCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeOutCirc:
											LeanTween.val = LeanTween.easeOutCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInOutCirc:
											LeanTween.val = LeanTween.easeInOutCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInBounce:
											LeanTween.val = LeanTween.easeInBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeOutBounce:
											LeanTween.val = LeanTween.easeOutBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInOutBounce:
											LeanTween.val = LeanTween.easeInOutBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeInBack:
											LeanTween.val = LeanTween.easeInBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot);
											break;
										case LeanTweenType.easeOutBack:
											LeanTween.val = LeanTween.easeOutBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot);
											break;
										case LeanTweenType.easeInOutBack:
											LeanTween.val = LeanTween.easeInOutBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot);
											break;
										case LeanTweenType.easeInElastic:
											LeanTween.val = LeanTween.easeInElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period);
											break;
										case LeanTweenType.easeOutElastic:
											LeanTween.val = LeanTween.easeOutElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period);
											break;
										case LeanTweenType.easeInOutElastic:
											LeanTween.val = LeanTween.easeInOutElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period);
											break;
										case LeanTweenType.easeSpring:
											LeanTween.val = LeanTween.spring(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed);
											break;
										case LeanTweenType.easeShake:
										case LeanTweenType.punch:
											if (LeanTween.tween.tweenType == LeanTweenType.punch)
											{
												LeanTween.tween.animationCurve = LeanTween.punch;
											}
											else if (LeanTween.tween.tweenType == LeanTweenType.easeShake)
											{
												LeanTween.tween.animationCurve = LeanTween.shake;
											}
											LeanTween.tween.to.x = LeanTween.tween.from.x + LeanTween.tween.to.x;
											LeanTween.tween.diff.x = LeanTween.tween.to.x - LeanTween.tween.from.x;
											LeanTween.val = LeanTween.tweenOnCurve(LeanTween.tween, LeanTween.ratioPassed);
											break;
										default:
											LeanTween.val = LeanTween.tween.from.x + LeanTween.tween.diff.x * LeanTween.ratioPassed;
											break;
										}
									}
									if (LeanTween.tweenAction == TweenAction.MOVE_X)
									{
										LeanTween.trans.set_position(new Vector3(LeanTween.val, LeanTween.trans.get_position().y, LeanTween.trans.get_position().z));
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_Y)
									{
										LeanTween.trans.set_position(new Vector3(LeanTween.trans.get_position().x, LeanTween.val, LeanTween.trans.get_position().z));
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_Z)
									{
										LeanTween.trans.set_position(new Vector3(LeanTween.trans.get_position().x, LeanTween.trans.get_position().y, LeanTween.val));
									}
									if (LeanTween.tweenAction == TweenAction.MOVE_LOCAL_X)
									{
										LeanTween.trans.set_localPosition(new Vector3(LeanTween.val, LeanTween.trans.get_localPosition().y, LeanTween.trans.get_localPosition().z));
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_LOCAL_Y)
									{
										LeanTween.trans.set_localPosition(new Vector3(LeanTween.trans.get_localPosition().x, LeanTween.val, LeanTween.trans.get_localPosition().z));
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_LOCAL_Z)
									{
										LeanTween.trans.set_localPosition(new Vector3(LeanTween.trans.get_localPosition().x, LeanTween.trans.get_localPosition().y, LeanTween.val));
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_CURVED)
									{
										if (LeanTween.tween.path.orientToPath)
										{
											if (LeanTween.tween.path.orientToPath2d)
											{
												LeanTween.tween.path.place2d(LeanTween.trans, LeanTween.val);
											}
											else
											{
												LeanTween.tween.path.place(LeanTween.trans, LeanTween.val);
											}
										}
										else
										{
											LeanTween.trans.set_position(LeanTween.tween.path.point(LeanTween.val));
										}
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_CURVED_LOCAL)
									{
										if (LeanTween.tween.path.orientToPath)
										{
											if (LeanTween.tween.path.orientToPath2d)
											{
												LeanTween.tween.path.placeLocal2d(LeanTween.trans, LeanTween.val);
											}
											else
											{
												LeanTween.tween.path.placeLocal(LeanTween.trans, LeanTween.val);
											}
										}
										else
										{
											LeanTween.trans.set_localPosition(LeanTween.tween.path.point(LeanTween.val));
										}
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_SPLINE)
									{
										if (LeanTween.tween.spline.orientToPath)
										{
											if (LeanTween.tween.spline.orientToPath2d)
											{
												LeanTween.tween.spline.place2d(LeanTween.trans, LeanTween.val);
											}
											else
											{
												LeanTween.tween.spline.place(LeanTween.trans, LeanTween.val);
											}
										}
										else
										{
											LeanTween.trans.set_position(LeanTween.tween.spline.point(LeanTween.val));
										}
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_SPLINE_LOCAL)
									{
										if (LeanTween.tween.spline.orientToPath)
										{
											if (LeanTween.tween.spline.orientToPath2d)
											{
												LeanTween.tween.spline.placeLocal2d(LeanTween.trans, LeanTween.val);
											}
											else
											{
												LeanTween.tween.spline.placeLocal(LeanTween.trans, LeanTween.val);
											}
										}
										else
										{
											LeanTween.trans.set_localPosition(LeanTween.tween.spline.point(LeanTween.val));
										}
									}
									else if (LeanTween.tweenAction == TweenAction.SCALE_X)
									{
										LeanTween.trans.set_localScale(new Vector3(LeanTween.val, LeanTween.trans.get_localScale().y, LeanTween.trans.get_localScale().z));
									}
									else if (LeanTween.tweenAction == TweenAction.SCALE_Y)
									{
										LeanTween.trans.set_localScale(new Vector3(LeanTween.trans.get_localScale().x, LeanTween.val, LeanTween.trans.get_localScale().z));
									}
									else if (LeanTween.tweenAction == TweenAction.SCALE_Z)
									{
										LeanTween.trans.set_localScale(new Vector3(LeanTween.trans.get_localScale().x, LeanTween.trans.get_localScale().y, LeanTween.val));
									}
									else if (LeanTween.tweenAction == TweenAction.ROTATE_X)
									{
										LeanTween.trans.set_eulerAngles(new Vector3(LeanTween.val, LeanTween.trans.get_eulerAngles().y, LeanTween.trans.get_eulerAngles().z));
									}
									else if (LeanTween.tweenAction == TweenAction.ROTATE_Y)
									{
										LeanTween.trans.set_eulerAngles(new Vector3(LeanTween.trans.get_eulerAngles().x, LeanTween.val, LeanTween.trans.get_eulerAngles().z));
									}
									else if (LeanTween.tweenAction == TweenAction.ROTATE_Z)
									{
										LeanTween.trans.set_eulerAngles(new Vector3(LeanTween.trans.get_eulerAngles().x, LeanTween.trans.get_eulerAngles().y, LeanTween.val));
									}
									else if (LeanTween.tweenAction == TweenAction.ROTATE_AROUND)
									{
										Vector3 localPosition = LeanTween.trans.get_localPosition();
										Vector3 vector = LeanTween.trans.TransformPoint(LeanTween.tween.point);
										LeanTween.trans.RotateAround(vector, LeanTween.tween.axis, -LeanTween.val);
										Vector3 vector2 = localPosition - LeanTween.trans.get_localPosition();
										LeanTween.trans.set_localPosition(localPosition - vector2);
										LeanTween.trans.set_rotation(LeanTween.tween.origRotation);
										vector = LeanTween.trans.TransformPoint(LeanTween.tween.point);
										LeanTween.trans.RotateAround(vector, LeanTween.tween.axis, LeanTween.val);
									}
									else if (LeanTween.tweenAction == TweenAction.ROTATE_AROUND_LOCAL)
									{
										Vector3 localPosition2 = LeanTween.trans.get_localPosition();
										LeanTween.trans.RotateAround(LeanTween.trans.TransformPoint(LeanTween.tween.point), LeanTween.trans.TransformDirection(LeanTween.tween.axis), -LeanTween.val);
										Vector3 vector3 = localPosition2 - LeanTween.trans.get_localPosition();
										LeanTween.trans.set_localPosition(localPosition2 - vector3);
										LeanTween.trans.set_localRotation(LeanTween.tween.origRotation);
										Vector3 vector4 = LeanTween.trans.TransformPoint(LeanTween.tween.point);
										LeanTween.trans.RotateAround(vector4, LeanTween.trans.TransformDirection(LeanTween.tween.axis), LeanTween.val);
									}
									else if (LeanTween.tweenAction == TweenAction.ALPHA)
									{
										SpriteRenderer component = LeanTween.trans.get_gameObject().GetComponent<SpriteRenderer>();
										if (component != null)
										{
											component.set_color(new Color(component.get_color().r, component.get_color().g, component.get_color().b, LeanTween.val));
										}
										else
										{
											if (LeanTween.trans.get_gameObject().GetComponent<Renderer>() != null)
											{
												Material[] materials = LeanTween.trans.get_gameObject().GetComponent<Renderer>().get_materials();
												for (int i = 0; i < materials.Length; i++)
												{
													Material material = materials[i];
													if (material.HasProperty("_Color"))
													{
														material.set_color(new Color(material.get_color().r, material.get_color().g, material.get_color().b, LeanTween.val));
													}
													else if (material.HasProperty("_TintColor"))
													{
														Color color = material.GetColor("_TintColor");
														material.SetColor("_TintColor", new Color(color.r, color.g, color.b, LeanTween.val));
													}
												}
											}
											if (LeanTween.trans.get_childCount() > 0)
											{
												using (IEnumerator enumerator = LeanTween.trans.GetEnumerator())
												{
													while (enumerator.MoveNext())
													{
														Transform transform = (Transform)enumerator.get_Current();
														if (transform.get_gameObject().GetComponent<Renderer>() != null)
														{
															Material[] materials2 = transform.get_gameObject().GetComponent<Renderer>().get_materials();
															for (int j = 0; j < materials2.Length; j++)
															{
																Material material2 = materials2[j];
																material2.set_color(new Color(material2.get_color().r, material2.get_color().g, material2.get_color().b, LeanTween.val));
															}
														}
													}
												}
											}
										}
									}
									else if (LeanTween.tweenAction == TweenAction.ALPHA_VERTEX)
									{
										Mesh mesh = LeanTween.trans.GetComponent<MeshFilter>().get_mesh();
										Vector3[] vertices = mesh.get_vertices();
										Color32[] array = new Color32[vertices.Length];
										Color32 color2 = mesh.get_colors32()[0];
										color2 = new Color((float)color2.r, (float)color2.g, (float)color2.b, LeanTween.val);
										for (int k = 0; k < vertices.Length; k++)
										{
											array[k] = color2;
										}
										mesh.set_colors32(array);
									}
									else if (LeanTween.tweenAction == TweenAction.COLOR || LeanTween.tweenAction == TweenAction.CALLBACK_COLOR)
									{
										Color color3 = LeanTween.tweenColor(LeanTween.tween, LeanTween.val);
										SpriteRenderer component2 = LeanTween.trans.get_gameObject().GetComponent<SpriteRenderer>();
										if (component2 != null)
										{
											component2.set_color(color3);
										}
										else if (LeanTween.tweenAction == TweenAction.COLOR)
										{
											if (LeanTween.trans.get_gameObject().GetComponent<Renderer>() != null)
											{
												Material[] materials3 = LeanTween.trans.get_gameObject().GetComponent<Renderer>().get_materials();
												for (int l = 0; l < materials3.Length; l++)
												{
													Material material3 = materials3[l];
													material3.set_color(color3);
												}
											}
											if (LeanTween.trans.get_childCount() > 0)
											{
												using (IEnumerator enumerator2 = LeanTween.trans.GetEnumerator())
												{
													while (enumerator2.MoveNext())
													{
														Transform transform2 = (Transform)enumerator2.get_Current();
														if (transform2.get_gameObject().GetComponent<Renderer>() != null)
														{
															Material[] materials4 = transform2.get_gameObject().GetComponent<Renderer>().get_materials();
															for (int m = 0; m < materials4.Length; m++)
															{
																Material material4 = materials4[m];
																material4.set_color(color3);
															}
														}
													}
												}
											}
										}
										if (LeanTween.dt != 0f && LeanTween.tween.onUpdateColor != null)
										{
											LeanTween.tween.onUpdateColor.Invoke(color3);
										}
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_ALPHA)
									{
										Color color4 = LeanTween.tween.uiImage.get_color();
										color4.a = LeanTween.val;
										LeanTween.tween.uiImage.set_color(color4);
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_COLOR)
									{
										Color color5 = LeanTween.tweenColor(LeanTween.tween, LeanTween.val);
										LeanTween.tween.uiImage.set_color(color5);
										if (LeanTween.dt != 0f && LeanTween.tween.onUpdateColor != null)
										{
											LeanTween.tween.onUpdateColor.Invoke(color5);
										}
									}
									else if (LeanTween.tweenAction == TweenAction.TEXT_ALPHA)
									{
										LeanTween.textAlphaRecursive(LeanTween.trans, LeanTween.val);
									}
									else if (LeanTween.tweenAction == TweenAction.TEXT_COLOR)
									{
										Color color6 = LeanTween.tweenColor(LeanTween.tween, LeanTween.val);
										LeanTween.tween.uiText.set_color(color6);
										if (LeanTween.dt != 0f && LeanTween.tween.onUpdateColor != null)
										{
											LeanTween.tween.onUpdateColor.Invoke(color6);
										}
										if (LeanTween.trans.get_childCount() > 0)
										{
											using (IEnumerator enumerator3 = LeanTween.trans.GetEnumerator())
											{
												while (enumerator3.MoveNext())
												{
													Transform transform3 = (Transform)enumerator3.get_Current();
													Text component3 = transform3.get_gameObject().GetComponent<Text>();
													if (component3 != null)
													{
														component3.set_color(color6);
													}
												}
											}
										}
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_ROTATEAROUND)
									{
										RectTransform rectTransform = LeanTween.tween.rectTransform;
										Vector3 localPosition3 = rectTransform.get_localPosition();
										rectTransform.RotateAround(rectTransform.TransformPoint(LeanTween.tween.point), LeanTween.tween.axis, -LeanTween.val);
										Vector3 vector5 = localPosition3 - rectTransform.get_localPosition();
										rectTransform.set_localPosition(localPosition3 - vector5);
										rectTransform.set_rotation(LeanTween.tween.origRotation);
										rectTransform.RotateAround(rectTransform.TransformPoint(LeanTween.tween.point), LeanTween.tween.axis, LeanTween.val);
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_ROTATEAROUND_LOCAL)
									{
										RectTransform rectTransform2 = LeanTween.tween.rectTransform;
										Vector3 localPosition4 = rectTransform2.get_localPosition();
										rectTransform2.RotateAround(rectTransform2.TransformPoint(LeanTween.tween.point), rectTransform2.TransformDirection(LeanTween.tween.axis), -LeanTween.val);
										Vector3 vector6 = localPosition4 - rectTransform2.get_localPosition();
										rectTransform2.set_localPosition(localPosition4 - vector6);
										rectTransform2.set_rotation(LeanTween.tween.origRotation);
										rectTransform2.RotateAround(rectTransform2.TransformPoint(LeanTween.tween.point), rectTransform2.TransformDirection(LeanTween.tween.axis), LeanTween.val);
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_PLAYSPRITE)
									{
										int num2 = (int)Mathf.Round(LeanTween.val);
										LeanTween.tween.uiImage.set_sprite(LeanTween.tween.sprites[num2]);
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_MOVE_X)
									{
										Vector3 anchoredPosition3D = LeanTween.tween.rectTransform.get_anchoredPosition3D();
										LeanTween.tween.rectTransform.set_anchoredPosition3D(new Vector3(LeanTween.val, anchoredPosition3D.y, anchoredPosition3D.z));
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_MOVE_Y)
									{
										Vector3 anchoredPosition3D2 = LeanTween.tween.rectTransform.get_anchoredPosition3D();
										LeanTween.tween.rectTransform.set_anchoredPosition3D(new Vector3(anchoredPosition3D2.x, LeanTween.val, anchoredPosition3D2.z));
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_MOVE_Z)
									{
										Vector3 anchoredPosition3D3 = LeanTween.tween.rectTransform.get_anchoredPosition3D();
										LeanTween.tween.rectTransform.set_anchoredPosition3D(new Vector3(anchoredPosition3D3.x, anchoredPosition3D3.y, LeanTween.val));
									}
								}
								else if (LeanTween.tweenAction >= TweenAction.MOVE)
								{
									if (LeanTween.tween.animationCurve != null)
									{
										LeanTween.newVect = LeanTween.tweenOnCurveVector(LeanTween.tween, LeanTween.ratioPassed);
									}
									else if (LeanTween.tween.tweenType == LeanTweenType.linear)
									{
										LeanTween.newVect = new Vector3(LeanTween.tween.from.x + LeanTween.tween.diff.x * LeanTween.ratioPassed, LeanTween.tween.from.y + LeanTween.tween.diff.y * LeanTween.ratioPassed, LeanTween.tween.from.z + LeanTween.tween.diff.z * LeanTween.ratioPassed);
									}
									else if (LeanTween.tween.tweenType >= LeanTweenType.linear)
									{
										switch (LeanTween.tween.tweenType)
										{
										case LeanTweenType.easeOutQuad:
											LeanTween.newVect = new Vector3(LeanTween.easeOutQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed), LeanTween.easeOutQuadOpt(LeanTween.tween.from.y, LeanTween.tween.diff.y, LeanTween.ratioPassed), LeanTween.easeOutQuadOpt(LeanTween.tween.from.z, LeanTween.tween.diff.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInQuad:
											LeanTween.newVect = new Vector3(LeanTween.easeInQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed), LeanTween.easeInQuadOpt(LeanTween.tween.from.y, LeanTween.tween.diff.y, LeanTween.ratioPassed), LeanTween.easeInQuadOpt(LeanTween.tween.from.z, LeanTween.tween.diff.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInOutQuad:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutQuadOpt(LeanTween.tween.from.x, LeanTween.tween.diff.x, LeanTween.ratioPassed), LeanTween.easeInOutQuadOpt(LeanTween.tween.from.y, LeanTween.tween.diff.y, LeanTween.ratioPassed), LeanTween.easeInOutQuadOpt(LeanTween.tween.from.z, LeanTween.tween.diff.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInCubic:
											LeanTween.newVect = new Vector3(LeanTween.easeInCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInCubic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInCubic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeOutCubic:
											LeanTween.newVect = new Vector3(LeanTween.easeOutCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutCubic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutCubic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInOutCubic:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutCubic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutCubic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutCubic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInQuart:
											LeanTween.newVect = new Vector3(LeanTween.easeInQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInQuart(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInQuart(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeOutQuart:
											LeanTween.newVect = new Vector3(LeanTween.easeOutQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutQuart(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutQuart(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInOutQuart:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutQuart(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutQuart(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutQuart(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInQuint:
											LeanTween.newVect = new Vector3(LeanTween.easeInQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInQuint(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInQuint(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeOutQuint:
											LeanTween.newVect = new Vector3(LeanTween.easeOutQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutQuint(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutQuint(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInOutQuint:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutQuint(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutQuint(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutQuint(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInSine:
											LeanTween.newVect = new Vector3(LeanTween.easeInSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInSine(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInSine(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeOutSine:
											LeanTween.newVect = new Vector3(LeanTween.easeOutSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutSine(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutSine(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInOutSine:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutSine(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutSine(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutSine(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInExpo:
											LeanTween.newVect = new Vector3(LeanTween.easeInExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInExpo(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInExpo(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeOutExpo:
											LeanTween.newVect = new Vector3(LeanTween.easeOutExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutExpo(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutExpo(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInOutExpo:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutExpo(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutExpo(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutExpo(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInCirc:
											LeanTween.newVect = new Vector3(LeanTween.easeInCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInCirc(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInCirc(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeOutCirc:
											LeanTween.newVect = new Vector3(LeanTween.easeOutCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutCirc(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutCirc(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInOutCirc:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutCirc(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutCirc(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutCirc(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInBounce:
											LeanTween.newVect = new Vector3(LeanTween.easeInBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInBounce(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInBounce(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeOutBounce:
											LeanTween.newVect = new Vector3(LeanTween.easeOutBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeOutBounce(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeOutBounce(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInOutBounce:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutBounce(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.easeInOutBounce(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.easeInOutBounce(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeInBack:
											LeanTween.newVect = new Vector3(LeanTween.easeInBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, 1f), LeanTween.easeInBack(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, 1f), LeanTween.easeInBack(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, 1f));
											break;
										case LeanTweenType.easeOutBack:
											LeanTween.newVect = new Vector3(LeanTween.easeOutBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot), LeanTween.easeOutBack(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot), LeanTween.easeOutBack(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot));
											break;
										case LeanTweenType.easeInOutBack:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutBack(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot), LeanTween.easeInOutBack(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot), LeanTween.easeInOutBack(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot));
											break;
										case LeanTweenType.easeInElastic:
											LeanTween.newVect = new Vector3(LeanTween.easeInElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeInElastic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeInElastic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period));
											break;
										case LeanTweenType.easeOutElastic:
											LeanTween.newVect = new Vector3(LeanTween.easeOutElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeOutElastic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeOutElastic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period));
											break;
										case LeanTweenType.easeInOutElastic:
											LeanTween.newVect = new Vector3(LeanTween.easeInOutElastic(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeInOutElastic(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period), LeanTween.easeInOutElastic(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed, LeanTween.tween.overshoot, LeanTween.tween.period));
											break;
										case LeanTweenType.easeSpring:
											LeanTween.newVect = new Vector3(LeanTween.spring(LeanTween.tween.from.x, LeanTween.tween.to.x, LeanTween.ratioPassed), LeanTween.spring(LeanTween.tween.from.y, LeanTween.tween.to.y, LeanTween.ratioPassed), LeanTween.spring(LeanTween.tween.from.z, LeanTween.tween.to.z, LeanTween.ratioPassed));
											break;
										case LeanTweenType.easeShake:
										case LeanTweenType.punch:
											if (LeanTween.tween.tweenType == LeanTweenType.punch)
											{
												LeanTween.tween.animationCurve = LeanTween.punch;
											}
											else if (LeanTween.tween.tweenType == LeanTweenType.easeShake)
											{
												LeanTween.tween.animationCurve = LeanTween.shake;
											}
											LeanTween.tween.to = LeanTween.tween.from + LeanTween.tween.to;
											LeanTween.tween.diff = LeanTween.tween.to - LeanTween.tween.from;
											if (LeanTween.tweenAction == TweenAction.ROTATE || LeanTween.tweenAction == TweenAction.ROTATE_LOCAL)
											{
												LeanTween.tween.to = new Vector3(LeanTween.closestRot(LeanTween.tween.from.x, LeanTween.tween.to.x), LeanTween.closestRot(LeanTween.tween.from.y, LeanTween.tween.to.y), LeanTween.closestRot(LeanTween.tween.from.z, LeanTween.tween.to.z));
											}
											LeanTween.newVect = LeanTween.tweenOnCurveVector(LeanTween.tween, LeanTween.ratioPassed);
											break;
										}
									}
									else
									{
										LeanTween.newVect = new Vector3(LeanTween.tween.from.x + LeanTween.tween.diff.x * LeanTween.ratioPassed, LeanTween.tween.from.y + LeanTween.tween.diff.y * LeanTween.ratioPassed, LeanTween.tween.from.z + LeanTween.tween.diff.z * LeanTween.ratioPassed);
									}
									if (LeanTween.tweenAction == TweenAction.MOVE)
									{
										LeanTween.trans.set_position(LeanTween.newVect);
									}
									else if (LeanTween.tweenAction == TweenAction.MOVE_LOCAL)
									{
										LeanTween.trans.set_localPosition(LeanTween.newVect);
									}
									else if (LeanTween.tweenAction == TweenAction.ROTATE)
									{
										LeanTween.trans.set_eulerAngles(LeanTween.newVect);
									}
									else if (LeanTween.tweenAction == TweenAction.ROTATE_LOCAL)
									{
										LeanTween.trans.set_localEulerAngles(LeanTween.newVect);
									}
									else if (LeanTween.tweenAction == TweenAction.SCALE)
									{
										LeanTween.trans.set_localScale(LeanTween.newVect);
									}
									else if (LeanTween.tweenAction == TweenAction.GUI_MOVE)
									{
										LeanTween.tween.ltRect.rect = new Rect(LeanTween.newVect.x, LeanTween.newVect.y, LeanTween.tween.ltRect.rect.get_width(), LeanTween.tween.ltRect.rect.get_height());
									}
									else if (LeanTween.tweenAction == TweenAction.GUI_MOVE_MARGIN)
									{
										LeanTween.tween.ltRect.margin = new Vector2(LeanTween.newVect.x, LeanTween.newVect.y);
									}
									else if (LeanTween.tweenAction == TweenAction.GUI_SCALE)
									{
										LeanTween.tween.ltRect.rect = new Rect(LeanTween.tween.ltRect.rect.get_x(), LeanTween.tween.ltRect.rect.get_y(), LeanTween.newVect.x, LeanTween.newVect.y);
									}
									else if (LeanTween.tweenAction == TweenAction.GUI_ALPHA)
									{
										LeanTween.tween.ltRect.alpha = LeanTween.newVect.x;
									}
									else if (LeanTween.tweenAction == TweenAction.GUI_ROTATE)
									{
										LeanTween.tween.ltRect.rotation = LeanTween.newVect.x;
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_MOVE)
									{
										LeanTween.tween.rectTransform.set_anchoredPosition3D(LeanTween.newVect);
									}
									else if (LeanTween.tweenAction == TweenAction.CANVAS_SCALE)
									{
										LeanTween.tween.rectTransform.set_localScale(LeanTween.newVect);
									}
								}
								if (LeanTween.dt != 0f && LeanTween.tween.hasUpdateCallback)
								{
									if (LeanTween.tween.onUpdateFloat != null)
									{
										LeanTween.tween.onUpdateFloat.Invoke(LeanTween.val);
									}
									if (LeanTween.tween.onUpdateFloatRatio != null)
									{
										LeanTween.tween.onUpdateFloatRatio.Invoke(LeanTween.val, LeanTween.ratioPassed);
									}
									else if (LeanTween.tween.onUpdateFloatObject != null)
									{
										LeanTween.tween.onUpdateFloatObject.Invoke(LeanTween.val, LeanTween.tween.onUpdateParam);
									}
									else if (LeanTween.tween.onUpdateVector3Object != null)
									{
										LeanTween.tween.onUpdateVector3Object.Invoke(LeanTween.newVect, LeanTween.tween.onUpdateParam);
									}
									else if (LeanTween.tween.onUpdateVector3 != null)
									{
										LeanTween.tween.onUpdateVector3.Invoke(LeanTween.newVect);
									}
									else if (LeanTween.tween.onUpdateVector2 != null)
									{
										LeanTween.tween.onUpdateVector2.Invoke(new Vector2(LeanTween.newVect.x, LeanTween.newVect.y));
									}
								}
							}
							if (LeanTween.isTweenFinished)
							{
								if (LeanTween.tween.loopType == LeanTweenType.once || LeanTween.tween.loopCount == 1)
								{
									LeanTween.tweensFinished[LeanTween.finishedCnt] = num;
									LeanTween.finishedCnt++;
									if (LeanTween.tweenAction == TweenAction.GUI_ROTATE)
									{
										LeanTween.tween.ltRect.rotateFinished = true;
									}
									if (LeanTween.tweenAction == TweenAction.DELAYED_SOUND)
									{
										AudioSource.PlayClipAtPoint((AudioClip)LeanTween.tween.onCompleteParam, LeanTween.tween.to, LeanTween.tween.from.x);
									}
								}
								else
								{
									if ((LeanTween.tween.loopCount < 0 && LeanTween.tween.type == TweenAction.CALLBACK) || LeanTween.tween.onCompleteOnRepeat)
									{
										if (LeanTween.tweenAction == TweenAction.DELAYED_SOUND)
										{
											AudioSource.PlayClipAtPoint((AudioClip)LeanTween.tween.onCompleteParam, LeanTween.tween.to, LeanTween.tween.from.x);
										}
										if (LeanTween.tween.onComplete != null)
										{
											LeanTween.tween.onComplete.Invoke();
										}
										else if (LeanTween.tween.onCompleteObject != null)
										{
											LeanTween.tween.onCompleteObject.Invoke(LeanTween.tween.onCompleteParam);
										}
									}
									if (LeanTween.tween.loopCount >= 1)
									{
										LeanTween.tween.loopCount--;
									}
									if (LeanTween.tween.loopType == LeanTweenType.pingPong)
									{
										LeanTween.tween.direction = 0f - LeanTween.tween.direction;
									}
									else
									{
										LeanTween.tween.passed = Mathf.Epsilon;
									}
								}
							}
							else if (LeanTween.tween.delay <= 0f)
							{
								LeanTween.tween.passed += LeanTween.dt * LeanTween.tween.direction;
							}
							else
							{
								LeanTween.tween.delay -= LeanTween.dt;
								if (LeanTween.tween.delay < 0f)
								{
									LeanTween.tween.passed = 0f;
									LeanTween.tween.delay = 0f;
								}
							}
						}
					}
					num++;
				}
				LeanTween.tweenMaxSearch = LeanTween.maxTweenReached;
				LeanTween.frameRendered = Time.get_frameCount();
				for (int n = 0; n < LeanTween.finishedCnt; n++)
				{
					LeanTween.j = LeanTween.tweensFinished[n];
					LeanTween.tween = LeanTween.tweens[LeanTween.j];
					if (LeanTween.tween.onComplete != null)
					{
						Action onComplete = LeanTween.tween.onComplete;
						LeanTween.removeTween(LeanTween.j);
						onComplete.Invoke();
					}
					else if (LeanTween.tween.onCompleteObject != null)
					{
						Action<object> onCompleteObject = LeanTween.tween.onCompleteObject;
						object onCompleteParam = LeanTween.tween.onCompleteParam;
						LeanTween.removeTween(LeanTween.j);
						onCompleteObject.Invoke(onCompleteParam);
					}
					else
					{
						LeanTween.removeTween(LeanTween.j);
					}
				}
			}
		}

		private static void textAlphaRecursive(Transform trans, float val)
		{
			Text component = trans.get_gameObject().GetComponent<Text>();
			if (component != null)
			{
				Color color = component.get_color();
				color.a = val;
				component.set_color(color);
			}
			if (trans.get_childCount() > 0)
			{
				using (IEnumerator enumerator = trans.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Transform transform = (Transform)enumerator.get_Current();
						LeanTween.textAlphaRecursive(transform, val);
					}
				}
			}
		}

		private static Color tweenColor(LTDescr tween, float val)
		{
			Vector3 vector = tween.point - tween.axis;
			float num = tween.to.y - tween.from.y;
			return new Color(tween.axis.x + vector.x * val, tween.axis.y + vector.y * val, tween.axis.z + vector.z * val, tween.from.y + num * val);
		}

		public static void removeTween(int i, int uniqueId)
		{
			if (LeanTween.tweens[i].uniqueId == uniqueId)
			{
				LeanTween.removeTween(i);
			}
		}

		public static void removeTween(int i)
		{
			if (LeanTween.tweens[i].toggle)
			{
				LeanTween.tweens[i].toggle = false;
				if (LeanTween.tweens[i].destroyOnComplete)
				{
					if (LeanTween.tweens[i].ltRect != null)
					{
						LTGUI.destroy(LeanTween.tweens[i].ltRect.id);
					}
					else if (LeanTween.tweens[i].trans.get_gameObject() != LeanTween._tweenEmpty)
					{
						Object.Destroy(LeanTween.tweens[i].trans.get_gameObject());
					}
				}
				LeanTween.tweens[i].cleanup();
				LeanTween.startSearch = i;
				if (i + 1 >= LeanTween.tweenMaxSearch)
				{
					LeanTween.startSearch = 0;
				}
			}
		}

		public static Vector3[] add(Vector3[] a, Vector3 b)
		{
			Vector3[] array = new Vector3[a.Length];
			LeanTween.i = 0;
			while (LeanTween.i < a.Length)
			{
				array[LeanTween.i] = a[LeanTween.i] + b;
				LeanTween.i++;
			}
			return array;
		}

		public static float closestRot(float from, float to)
		{
			float num = 0f - (360f - to);
			float num2 = 360f + to;
			float num3 = Mathf.Abs(to - from);
			float num4 = Mathf.Abs(num - from);
			float num5 = Mathf.Abs(num2 - from);
			if (num3 < num4 && num3 < num5)
			{
				return to;
			}
			if (num4 < num5)
			{
				return num;
			}
			return num2;
		}

		public static void cancelAll()
		{
			LeanTween.cancelAll(false);
		}

		public static void cancelAll(bool callComplete)
		{
			LeanTween.init();
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].trans != null)
				{
					if (callComplete && LeanTween.tweens[i].onComplete != null)
					{
						LeanTween.tweens[i].onComplete.Invoke();
					}
					LeanTween.removeTween(i);
				}
			}
		}

		public static void cancel(GameObject gameObject)
		{
			LeanTween.cancel(gameObject, false);
		}

		public static void cancel(GameObject gameObject, bool callOnComplete)
		{
			LeanTween.init();
			Transform transform = gameObject.get_transform();
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].trans == transform)
				{
					if (callOnComplete && LeanTween.tweens[i].onComplete != null)
					{
						LeanTween.tweens[i].onComplete.Invoke();
					}
					LeanTween.removeTween(i);
				}
			}
		}

		public static void cancel(GameObject gameObject, int uniqueId)
		{
			if (uniqueId >= 0)
			{
				LeanTween.init();
				int num = uniqueId & 65535;
				int num2 = uniqueId >> 16;
				if (LeanTween.tweens[num].trans == null || (LeanTween.tweens[num].trans.get_gameObject() == gameObject && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2)))
				{
					LeanTween.removeTween(num);
				}
			}
		}

		public static void cancel(LTRect ltRect, int uniqueId)
		{
			if (uniqueId >= 0)
			{
				LeanTween.init();
				int num = uniqueId & 65535;
				int num2 = uniqueId >> 16;
				if (LeanTween.tweens[num].ltRect == ltRect && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
				{
					LeanTween.removeTween(num);
				}
			}
		}

		public static void cancel(int uniqueId)
		{
			LeanTween.cancel(uniqueId, false);
		}

		public static void cancel(int uniqueId, bool callOnComplete)
		{
			if (uniqueId >= 0)
			{
				LeanTween.init();
				int num = uniqueId & 65535;
				int num2 = uniqueId >> 16;
				if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
				{
					if (callOnComplete && LeanTween.tweens[num].onComplete != null)
					{
						LeanTween.tweens[num].onComplete.Invoke();
					}
					LeanTween.removeTween(num);
				}
			}
		}

		public static LTDescr descr(int uniqueId)
		{
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if (LeanTween.tweens[num] != null && LeanTween.tweens[num].uniqueId == uniqueId && (ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				return LeanTween.tweens[num];
			}
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].uniqueId == uniqueId && (ulong)LeanTween.tweens[i].counter == (ulong)((long)num2))
				{
					return LeanTween.tweens[i];
				}
			}
			return null;
		}

		public static LTDescr description(int uniqueId)
		{
			return LeanTween.descr(uniqueId);
		}

		public static LTDescr[] descriptions(GameObject gameObject = null)
		{
			if (gameObject == null)
			{
				return null;
			}
			List<LTDescr> list = new List<LTDescr>();
			Transform transform = gameObject.get_transform();
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].trans == transform)
				{
					list.Add(LeanTween.tweens[i]);
				}
			}
			return list.ToArray();
		}

		[Obsolete("Use 'pause( id )' instead")]
		public static void pause(GameObject gameObject, int uniqueId)
		{
			LeanTween.pause(uniqueId);
		}

		public static void pause(int uniqueId)
		{
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				LeanTween.tweens[num].pause();
			}
		}

		public static void pause(GameObject gameObject)
		{
			Transform transform = gameObject.get_transform();
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].trans == transform)
				{
					LeanTween.tweens[i].pause();
				}
			}
		}

		public static void pauseAll()
		{
			LeanTween.init();
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				LeanTween.tweens[i].pause();
			}
		}

		public static void resumeAll()
		{
			LeanTween.init();
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				LeanTween.tweens[i].resume();
			}
		}

		[Obsolete("Use 'resume( id )' instead")]
		public static void resume(GameObject gameObject, int uniqueId)
		{
			LeanTween.resume(uniqueId);
		}

		public static void resume(int uniqueId)
		{
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			if ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2))
			{
				LeanTween.tweens[num].resume();
			}
		}

		public static void resume(GameObject gameObject)
		{
			Transform transform = gameObject.get_transform();
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].trans == transform)
				{
					LeanTween.tweens[i].resume();
				}
			}
		}

		public static bool isTweening(GameObject gameObject = null)
		{
			if (gameObject == null)
			{
				for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
				{
					if (LeanTween.tweens[i].toggle)
					{
						return true;
					}
				}
				return false;
			}
			Transform transform = gameObject.get_transform();
			for (int j = 0; j <= LeanTween.tweenMaxSearch; j++)
			{
				if (LeanTween.tweens[j].toggle && LeanTween.tweens[j].trans == transform)
				{
					return true;
				}
			}
			return false;
		}

		public static bool isTweening(int uniqueId)
		{
			int num = uniqueId & 65535;
			int num2 = uniqueId >> 16;
			return num >= 0 && num < LeanTween.maxTweens && ((ulong)LeanTween.tweens[num].counter == (ulong)((long)num2) && LeanTween.tweens[num].toggle);
		}

		public static bool isTweening(LTRect ltRect)
		{
			for (int i = 0; i <= LeanTween.tweenMaxSearch; i++)
			{
				if (LeanTween.tweens[i].toggle && LeanTween.tweens[i].ltRect == ltRect)
				{
					return true;
				}
			}
			return false;
		}

		public static void drawBezierPath(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float arrowSize = 0f, Transform arrowTransform = null)
		{
			Vector3 vector = a;
			Vector3 vector2 = -a + 3f * (b - c) + d;
			Vector3 vector3 = 3f * (a + c) - 6f * b;
			Vector3 vector4 = 3f * (b - a);
			if (arrowSize > 0f)
			{
				Vector3 position = arrowTransform.get_position();
				Quaternion rotation = arrowTransform.get_rotation();
				float num = 0f;
				for (float num2 = 1f; num2 <= 120f; num2 += 1f)
				{
					float num3 = num2 / 120f;
					Vector3 vector5 = ((vector2 * num3 + vector3) * num3 + vector4) * num3 + a;
					Gizmos.DrawLine(vector, vector5);
					num += (vector5 - vector).get_magnitude();
					if (num > 1f)
					{
						num -= 1f;
						arrowTransform.set_position(vector5);
						arrowTransform.LookAt(vector, Vector3.get_forward());
						Vector3 vector6 = arrowTransform.TransformDirection(Vector3.get_right());
						Vector3 normalized = (vector - vector5).get_normalized();
						Gizmos.DrawLine(vector5, vector5 + (vector6 + normalized) * arrowSize);
						vector6 = arrowTransform.TransformDirection(-Vector3.get_right());
						Gizmos.DrawLine(vector5, vector5 + (vector6 + normalized) * arrowSize);
					}
					vector = vector5;
				}
				arrowTransform.set_position(position);
				arrowTransform.set_rotation(rotation);
			}
			else
			{
				for (float num4 = 1f; num4 <= 30f; num4 += 1f)
				{
					float num3 = num4 / 30f;
					Vector3 vector5 = ((vector2 * num3 + vector3) * num3 + vector4) * num3 + a;
					Gizmos.DrawLine(vector, vector5);
					vector = vector5;
				}
			}
		}

		public static object logError(string error)
		{
			if (LeanTween.throwErrors)
			{
				Debug.LogError(error);
			}
			else
			{
				Debug.Log(error);
			}
			return null;
		}

		public static LTDescr options(LTDescr seed)
		{
			Debug.LogError("error this function is no longer used");
			return null;
		}

		public static LTDescr options()
		{
			LeanTween.init();
			bool flag = false;
			LeanTween.j = 0;
			LeanTween.i = LeanTween.startSearch;
			while (LeanTween.j < LeanTween.maxTweens)
			{
				if (LeanTween.i >= LeanTween.maxTweens - 1)
				{
					LeanTween.i = 0;
				}
				if (!LeanTween.tweens[LeanTween.i].toggle)
				{
					if (LeanTween.i + 1 > LeanTween.tweenMaxSearch)
					{
						LeanTween.tweenMaxSearch = LeanTween.i + 1;
					}
					LeanTween.startSearch = LeanTween.i + 1;
					flag = true;
					break;
				}
				LeanTween.j++;
				if (LeanTween.j >= LeanTween.maxTweens)
				{
					return LeanTween.logError("LeanTween - You have run out of available spaces for tweening. To avoid this error increase the number of spaces to available for tweening when you initialize the LeanTween class ex: LeanTween.init( " + LeanTween.maxTweens * 2 + " );") as LTDescr;
				}
				LeanTween.i++;
			}
			if (!flag)
			{
				LeanTween.logError("no available tween found!");
			}
			LeanTween.tweens[LeanTween.i].reset();
			LeanTween.tweens[LeanTween.i].setId((uint)LeanTween.i);
			return LeanTween.tweens[LeanTween.i];
		}

		private static LTDescr pushNewTween(GameObject gameObject, Vector3 to, float time, TweenAction tweenAction, LTDescr tween)
		{
			LeanTween.init(LeanTween.maxTweens);
			if (gameObject == null || tween == null)
			{
				return null;
			}
			tween.trans = gameObject.get_transform();
			tween.to = to;
			tween.time = time;
			tween.type = tweenAction;
			return tween;
		}

		public static LTDescr play(RectTransform rectTransform, Sprite[] sprites)
		{
			float num = 0.25f;
			float time = num * (float)sprites.Length;
			return LeanTween.pushNewTween(rectTransform.get_gameObject(), new Vector3((float)sprites.Length - 1f, 0f, 0f), time, TweenAction.CANVAS_PLAYSPRITE, LeanTween.options().setSprites(sprites).setRepeat(-1));
		}

		public static LTDescr alpha(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ALPHA, LeanTween.options());
		}

		public static LTDescr alpha(LTRect ltRect, float to, float time)
		{
			ltRect.alphaEnabled = true;
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, TweenAction.GUI_ALPHA, LeanTween.options().setRect(ltRect));
		}

		public static LTDescr textAlpha(RectTransform rectTransform, float to, float time)
		{
			return LeanTween.pushNewTween(rectTransform.get_gameObject(), new Vector3(to, 0f, 0f), time, TweenAction.TEXT_ALPHA, LeanTween.options());
		}

		public static LTDescr alphaVertex(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ALPHA_VERTEX, LeanTween.options());
		}

		public static LTDescr color(GameObject gameObject, Color to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.COLOR, LeanTween.options().setPoint(new Vector3(to.r, to.g, to.b)));
		}

		public static LTDescr textColor(RectTransform rectTransform, Color to, float time)
		{
			return LeanTween.pushNewTween(rectTransform.get_gameObject(), new Vector3(1f, to.a, 0f), time, TweenAction.TEXT_COLOR, LeanTween.options().setPoint(new Vector3(to.r, to.g, to.b)));
		}

		public static LTDescr delayedCall(float delayTime, Action callback)
		{
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.get_zero(), delayTime, TweenAction.CALLBACK, LeanTween.options().setOnComplete(callback));
		}

		public static LTDescr delayedCall(float delayTime, Action<object> callback)
		{
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.get_zero(), delayTime, TweenAction.CALLBACK, LeanTween.options().setOnComplete(callback));
		}

		public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action callback)
		{
			return LeanTween.pushNewTween(gameObject, Vector3.get_zero(), delayTime, TweenAction.CALLBACK, LeanTween.options().setOnComplete(callback));
		}

		public static LTDescr delayedCall(GameObject gameObject, float delayTime, Action<object> callback)
		{
			return LeanTween.pushNewTween(gameObject, Vector3.get_zero(), delayTime, TweenAction.CALLBACK, LeanTween.options().setOnComplete(callback));
		}

		public static LTDescr destroyAfter(LTRect rect, float delayTime)
		{
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, Vector3.get_zero(), delayTime, TweenAction.CALLBACK, LeanTween.options().setRect(rect).setDestroyOnComplete(true));
		}

		public static LTDescr move(GameObject gameObject, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, to, time, TweenAction.MOVE, LeanTween.options());
		}

		public static LTDescr move(GameObject gameObject, Vector2 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, gameObject.get_transform().get_position().z), time, TweenAction.MOVE, LeanTween.options());
		}

		public static LTDescr move(GameObject gameObject, Vector3[] to, float time)
		{
			LeanTween.d = LeanTween.options();
			if (LeanTween.d.path == null)
			{
				LeanTween.d.path = new LTBezierPath(to);
			}
			else
			{
				LeanTween.d.path.setPoints(to);
			}
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED, LeanTween.d);
		}

		public static LTDescr move(GameObject gameObject, LTBezierPath to, float time)
		{
			LeanTween.d = LeanTween.options();
			LeanTween.d.path = to;
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED, LeanTween.d);
		}

		public static LTDescr move(GameObject gameObject, LTSpline to, float time)
		{
			LeanTween.d = LeanTween.options();
			LeanTween.d.spline = to;
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE, LeanTween.d);
		}

		public static LTDescr moveSpline(GameObject gameObject, Vector3[] to, float time)
		{
			LeanTween.d = LeanTween.options();
			LeanTween.d.spline = new LTSpline(to);
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE, LeanTween.d);
		}

		public static LTDescr moveSplineLocal(GameObject gameObject, Vector3[] to, float time)
		{
			LeanTween.d = LeanTween.options();
			LeanTween.d.spline = new LTSpline(to);
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE_LOCAL, LeanTween.d);
		}

		public static LTDescr move(LTRect ltRect, Vector2 to, float time)
		{
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, TweenAction.GUI_MOVE, LeanTween.options().setRect(ltRect));
		}

		public static LTDescr moveMargin(LTRect ltRect, Vector2 to, float time)
		{
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, TweenAction.GUI_MOVE_MARGIN, LeanTween.options().setRect(ltRect));
		}

		public static LTDescr moveX(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_X, LeanTween.options());
		}

		public static LTDescr moveY(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_Y, LeanTween.options());
		}

		public static LTDescr moveZ(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_Z, LeanTween.options());
		}

		public static LTDescr moveLocal(GameObject gameObject, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, to, time, TweenAction.MOVE_LOCAL, LeanTween.options());
		}

		public static LTDescr moveLocal(GameObject gameObject, Vector3[] to, float time)
		{
			LeanTween.d = LeanTween.options();
			if (LeanTween.d.path == null)
			{
				LeanTween.d.path = new LTBezierPath(to);
			}
			else
			{
				LeanTween.d.path.setPoints(to);
			}
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED_LOCAL, LeanTween.d);
		}

		public static LTDescr moveLocalX(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_X, LeanTween.options());
		}

		public static LTDescr moveLocalY(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_Y, LeanTween.options());
		}

		public static LTDescr moveLocalZ(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.MOVE_LOCAL_Z, LeanTween.options());
		}

		public static LTDescr moveLocal(GameObject gameObject, LTBezierPath to, float time)
		{
			LeanTween.d = LeanTween.options();
			LeanTween.d.path = to;
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_CURVED_LOCAL, LeanTween.d);
		}

		public static LTDescr moveLocal(GameObject gameObject, LTSpline to, float time)
		{
			LeanTween.d = LeanTween.options();
			LeanTween.d.spline = to;
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, 0f, 0f), time, TweenAction.MOVE_SPLINE_LOCAL, LeanTween.d);
		}

		public static LTDescr rotate(GameObject gameObject, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, to, time, TweenAction.ROTATE, LeanTween.options());
		}

		public static LTDescr rotate(LTRect ltRect, float to, float time)
		{
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, new Vector3(to, 0f, 0f), time, TweenAction.GUI_ROTATE, LeanTween.options().setRect(ltRect));
		}

		public static LTDescr rotateLocal(GameObject gameObject, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, to, time, TweenAction.ROTATE_LOCAL, LeanTween.options());
		}

		public static LTDescr rotateX(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_X, LeanTween.options());
		}

		public static LTDescr rotateY(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_Y, LeanTween.options());
		}

		public static LTDescr rotateZ(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.ROTATE_Z, LeanTween.options());
		}

		public static LTDescr rotateAround(GameObject gameObject, Vector3 axis, float add, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, TweenAction.ROTATE_AROUND, LeanTween.options().setAxis(axis));
		}

		public static LTDescr rotateAroundLocal(GameObject gameObject, Vector3 axis, float add, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(add, 0f, 0f), time, TweenAction.ROTATE_AROUND_LOCAL, LeanTween.options().setAxis(axis));
		}

		public static LTDescr scale(GameObject gameObject, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, to, time, TweenAction.SCALE, LeanTween.options());
		}

		public static LTDescr scale(LTRect ltRect, Vector2 to, float time)
		{
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, to, time, TweenAction.GUI_SCALE, LeanTween.options().setRect(ltRect));
		}

		public static LTDescr scaleX(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_X, LeanTween.options());
		}

		public static LTDescr scaleY(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_Y, LeanTween.options());
		}

		public static LTDescr scaleZ(GameObject gameObject, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.SCALE_Z, LeanTween.options());
		}

		public static LTDescr value(GameObject gameObject, float from, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, LeanTween.options().setFrom(new Vector3(from, 0f, 0f)));
		}

		public static LTDescr value(GameObject gameObject, Vector2 from, Vector2 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, TweenAction.VALUE3, LeanTween.options().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)));
		}

		public static LTDescr value(GameObject gameObject, Vector3 from, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, to, time, TweenAction.VALUE3, LeanTween.options().setFrom(from));
		}

		public static LTDescr value(GameObject gameObject, Color from, Color to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.CALLBACK_COLOR, LeanTween.options().setPoint(new Vector3(to.r, to.g, to.b)).setFromColor(from).setHasInitialized(false));
		}

		public static LTDescr value(GameObject gameObject, Action<float> callOnUpdate, float from, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, LeanTween.options().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdate(callOnUpdate));
		}

		public static LTDescr value(GameObject gameObject, Action<Color> callOnUpdate, Color from, Color to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(1f, to.a, 0f), time, TweenAction.CALLBACK_COLOR, LeanTween.options().setPoint(new Vector3(to.r, to.g, to.b)).setAxis(new Vector3(from.r, from.g, from.b)).setFrom(new Vector3(0f, from.a, 0f)).setHasInitialized(false).setOnUpdateColor(callOnUpdate));
		}

		public static LTDescr value(GameObject gameObject, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to.x, to.y, 0f), time, TweenAction.VALUE3, LeanTween.options().setTo(new Vector3(to.x, to.y, 0f)).setFrom(new Vector3(from.x, from.y, 0f)).setOnUpdateVector2(callOnUpdate));
		}

		public static LTDescr value(GameObject gameObject, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(gameObject, to, time, TweenAction.VALUE3, LeanTween.options().setTo(to).setFrom(from).setOnUpdateVector3(callOnUpdate));
		}

		public static LTDescr value(GameObject gameObject, Action<float, object> callOnUpdate, float from, float to, float time)
		{
			return LeanTween.pushNewTween(gameObject, new Vector3(to, 0f, 0f), time, TweenAction.CALLBACK, LeanTween.options().setTo(new Vector3(to, 0f, 0f)).setFrom(new Vector3(from, 0f, 0f)).setOnUpdateObject(callOnUpdate));
		}

		public static LTDescr delayedSound(AudioClip audio, Vector3 pos, float volume)
		{
			return LeanTween.pushNewTween(LeanTween.tweenEmpty, pos, 0f, TweenAction.DELAYED_SOUND, LeanTween.options().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
		}

		public static LTDescr delayedSound(GameObject gameObject, AudioClip audio, Vector3 pos, float volume)
		{
			return LeanTween.pushNewTween(gameObject, pos, 0f, TweenAction.DELAYED_SOUND, LeanTween.options().setTo(pos).setFrom(new Vector3(volume, 0f, 0f)).setAudio(audio));
		}

		public static LTDescr move(RectTransform rectTrans, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), to, time, TweenAction.CANVAS_MOVE, LeanTween.options().setRect(rectTrans));
		}

		public static LTDescr moveX(RectTransform rectTrans, float to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_X, LeanTween.options().setRect(rectTrans));
		}

		public static LTDescr moveY(RectTransform rectTrans, float to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_Y, LeanTween.options().setRect(rectTrans));
		}

		public static LTDescr moveZ(RectTransform rectTrans, float to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_MOVE_Z, LeanTween.options().setRect(rectTrans));
		}

		public static LTDescr rotate(RectTransform rectTrans, float to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND, LeanTween.options().setRect(rectTrans).setAxis(Vector3.get_forward()));
		}

		public static LTDescr rotateAround(RectTransform rectTrans, Vector3 axis, float to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND, LeanTween.options().setRect(rectTrans).setAxis(axis));
		}

		public static LTDescr rotateAroundLocal(RectTransform rectTrans, Vector3 axis, float to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ROTATEAROUND_LOCAL, LeanTween.options().setRect(rectTrans).setAxis(axis));
		}

		public static LTDescr scale(RectTransform rectTrans, Vector3 to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), to, time, TweenAction.CANVAS_SCALE, LeanTween.options().setRect(rectTrans));
		}

		public static LTDescr alpha(RectTransform rectTrans, float to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), new Vector3(to, 0f, 0f), time, TweenAction.CANVAS_ALPHA, LeanTween.options().setRect(rectTrans));
		}

		public static LTDescr color(RectTransform rectTrans, Color to, float time)
		{
			return LeanTween.pushNewTween(rectTrans.get_gameObject(), new Vector3(1f, to.a, 0f), time, TweenAction.CANVAS_COLOR, LeanTween.options().setRect(rectTrans).setPoint(new Vector3(to.r, to.g, to.b)));
		}

		private static float tweenOnCurve(LTDescr tweenDescr, float ratioPassed)
		{
			return tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.animationCurve.Evaluate(ratioPassed);
		}

		private static Vector3 tweenOnCurveVector(LTDescr tweenDescr, float ratioPassed)
		{
			return new Vector3(tweenDescr.from.x + tweenDescr.diff.x * tweenDescr.animationCurve.Evaluate(ratioPassed), tweenDescr.from.y + tweenDescr.diff.y * tweenDescr.animationCurve.Evaluate(ratioPassed), tweenDescr.from.z + tweenDescr.diff.z * tweenDescr.animationCurve.Evaluate(ratioPassed));
		}

		private static float easeOutQuadOpt(float start, float diff, float ratioPassed)
		{
			return -diff * ratioPassed * (ratioPassed - 2f) + start;
		}

		private static float easeInQuadOpt(float start, float diff, float ratioPassed)
		{
			return diff * ratioPassed * ratioPassed + start;
		}

		private static float easeInOutQuadOpt(float start, float diff, float ratioPassed)
		{
			ratioPassed /= 0.5f;
			if (ratioPassed < 1f)
			{
				return diff / 2f * ratioPassed * ratioPassed + start;
			}
			ratioPassed -= 1f;
			return -diff / 2f * (ratioPassed * (ratioPassed - 2f) - 1f) + start;
		}

		private static float linear(float start, float end, float val)
		{
			return Mathf.Lerp(start, end, val);
		}

		private static float clerp(float start, float end, float val)
		{
			float num = 0f;
			float num2 = 360f;
			float num3 = Mathf.Abs((num2 - num) / 2f);
			float result;
			if (end - start < -num3)
			{
				float num4 = (num2 - start + end) * val;
				result = start + num4;
			}
			else if (end - start > num3)
			{
				float num4 = -(num2 - end + start) * val;
				result = start + num4;
			}
			else
			{
				result = start + (end - start) * val;
			}
			return result;
		}

		private static float spring(float start, float end, float val)
		{
			val = Mathf.Clamp01(val);
			val = (Mathf.Sin(val * 3.14159274f * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
			return start + (end - start) * val;
		}

		private static float easeInQuad(float start, float end, float val)
		{
			end -= start;
			return end * val * val + start;
		}

		private static float easeOutQuad(float start, float end, float val)
		{
			end -= start;
			return -end * val * (val - 2f) + start;
		}

		private static float easeInOutQuad(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val + start;
			}
			val -= 1f;
			return -end / 2f * (val * (val - 2f) - 1f) + start;
		}

		private static float easeInCubic(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val + start;
		}

		private static float easeOutCubic(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * (val * val * val + 1f) + start;
		}

		private static float easeInOutCubic(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val + start;
			}
			val -= 2f;
			return end / 2f * (val * val * val + 2f) + start;
		}

		private static float easeInQuart(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val * val + start;
		}

		private static float easeOutQuart(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return -end * (val * val * val * val - 1f) + start;
		}

		private static float easeInOutQuart(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val * val + start;
			}
			val -= 2f;
			return -end / 2f * (val * val * val * val - 2f) + start;
		}

		private static float easeInQuint(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val * val * val + start;
		}

		private static float easeOutQuint(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * (val * val * val * val * val + 1f) + start;
		}

		private static float easeInOutQuint(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val * val * val + start;
			}
			val -= 2f;
			return end / 2f * (val * val * val * val * val + 2f) + start;
		}

		private static float easeInSine(float start, float end, float val)
		{
			end -= start;
			return -end * Mathf.Cos(val / 1f * 1.57079637f) + end + start;
		}

		private static float easeOutSine(float start, float end, float val)
		{
			end -= start;
			return end * Mathf.Sin(val / 1f * 1.57079637f) + start;
		}

		private static float easeInOutSine(float start, float end, float val)
		{
			end -= start;
			return -end / 2f * (Mathf.Cos(3.14159274f * val / 1f) - 1f) + start;
		}

		private static float easeInExpo(float start, float end, float val)
		{
			end -= start;
			return end * Mathf.Pow(2f, 10f * (val / 1f - 1f)) + start;
		}

		private static float easeOutExpo(float start, float end, float val)
		{
			end -= start;
			return end * (-Mathf.Pow(2f, -10f * val / 1f) + 1f) + start;
		}

		private static float easeInOutExpo(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * Mathf.Pow(2f, 10f * (val - 1f)) + start;
			}
			val -= 1f;
			return end / 2f * (-Mathf.Pow(2f, -10f * val) + 2f) + start;
		}

		private static float easeInCirc(float start, float end, float val)
		{
			end -= start;
			return -end * (Mathf.Sqrt(1f - val * val) - 1f) + start;
		}

		private static float easeOutCirc(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * Mathf.Sqrt(1f - val * val) + start;
		}

		private static float easeInOutCirc(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return -end / 2f * (Mathf.Sqrt(1f - val * val) - 1f) + start;
			}
			val -= 2f;
			return end / 2f * (Mathf.Sqrt(1f - val * val) + 1f) + start;
		}

		private static float easeInBounce(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			return end - LeanTween.easeOutBounce(0f, end, num - val) + start;
		}

		private static float easeOutBounce(float start, float end, float val)
		{
			val /= 1f;
			end -= start;
			if (val < 0.363636374f)
			{
				return end * (7.5625f * val * val) + start;
			}
			if (val < 0.727272749f)
			{
				val -= 0.545454562f;
				return end * (7.5625f * val * val + 0.75f) + start;
			}
			if ((double)val < 0.90909090909090906)
			{
				val -= 0.8181818f;
				return end * (7.5625f * val * val + 0.9375f) + start;
			}
			val -= 0.954545438f;
			return end * (7.5625f * val * val + 0.984375f) + start;
		}

		private static float easeInOutBounce(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			if (val < num / 2f)
			{
				return LeanTween.easeInBounce(0f, end, val * 2f) * 0.5f + start;
			}
			return LeanTween.easeOutBounce(0f, end, val * 2f - num) * 0.5f + end * 0.5f + start;
		}

		private static float easeInBack(float start, float end, float val, float overshoot = 1f)
		{
			end -= start;
			val /= 1f;
			float num = 1.70158f * overshoot;
			return end * val * val * ((num + 1f) * val - num) + start;
		}

		private static float easeOutBack(float start, float end, float val, float overshoot = 1f)
		{
			float num = 1.70158f * overshoot;
			end -= start;
			val = val / 1f - 1f;
			return end * (val * val * ((num + 1f) * val + num) + 1f) + start;
		}

		private static float easeInOutBack(float start, float end, float val, float overshoot = 1f)
		{
			float num = 1.70158f * overshoot;
			end -= start;
			val /= 0.5f;
			if (val < 1f)
			{
				num *= 1.525f * overshoot;
				return end / 2f * (val * val * ((num + 1f) * val - num)) + start;
			}
			val -= 2f;
			num *= 1.525f * overshoot;
			return end / 2f * (val * val * ((num + 1f) * val + num) + 2f) + start;
		}

		private static float easeInElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
		{
			end -= start;
			float num = 0f;
			if (val == 0f)
			{
				return start;
			}
			if (val == 1f)
			{
				return start + end;
			}
			float num2;
			if (num == 0f || num < Mathf.Abs(end))
			{
				num = end;
				num2 = period / 4f;
			}
			else
			{
				num2 = period / 6.28318548f * Mathf.Asin(end / num);
			}
			if (overshoot > 1f && val > 0.6f)
			{
				overshoot = 1f + (1f - val) / 0.4f * (overshoot - 1f);
			}
			val -= 1f;
			return start - num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.28318548f / period) * overshoot;
		}

		private static float easeOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
		{
			end -= start;
			float num = 0f;
			if (val == 0f)
			{
				return start;
			}
			if (val == 1f)
			{
				return start + end;
			}
			float num2;
			if (num == 0f || num < Mathf.Abs(end))
			{
				num = end;
				num2 = period / 4f;
			}
			else
			{
				num2 = period / 6.28318548f * Mathf.Asin(end / num);
			}
			if (overshoot > 1f && val < 0.4f)
			{
				overshoot = 1f + val / 0.4f * (overshoot - 1f);
			}
			return start + end + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.28318548f / period) * overshoot;
		}

		private static float easeInOutElastic(float start, float end, float val, float overshoot = 1f, float period = 0.3f)
		{
			end -= start;
			float num = 0f;
			if (val == 0f)
			{
				return start;
			}
			val /= 0.5f;
			if (val == 2f)
			{
				return start + end;
			}
			float num2;
			if (num == 0f || num < Mathf.Abs(end))
			{
				num = end;
				num2 = period / 4f;
			}
			else
			{
				num2 = period / 6.28318548f * Mathf.Asin(end / num);
			}
			if (overshoot > 1f)
			{
				if (val < 0.2f)
				{
					overshoot = 1f + val / 0.2f * (overshoot - 1f);
				}
				else if (val > 0.8f)
				{
					overshoot = 1f + (1f - val) / 0.2f * (overshoot - 1f);
				}
			}
			if (val < 1f)
			{
				val -= 1f;
				return start - 0.5f * (num * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - num2) * 6.28318548f / period)) * overshoot;
			}
			val -= 1f;
			return end + start + num * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - num2) * 6.28318548f / period) * 0.5f * overshoot;
		}

		public static void addListener(int eventId, Action<LTEvent> callback)
		{
			LeanTween.addListener(LeanTween.tweenEmpty, eventId, callback);
		}

		public static void addListener(GameObject caller, int eventId, Action<LTEvent> callback)
		{
			if (LeanTween.eventListeners == null)
			{
				LeanTween.INIT_LISTENERS_MAX = LeanTween.LISTENERS_MAX;
				LeanTween.eventListeners = new Action<LTEvent>[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
				LeanTween.goListeners = new GameObject[LeanTween.EVENTS_MAX * LeanTween.LISTENERS_MAX];
			}
			LeanTween.i = 0;
			while (LeanTween.i < LeanTween.INIT_LISTENERS_MAX)
			{
				int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
				if (LeanTween.goListeners[num] == null || LeanTween.eventListeners[num] == null)
				{
					LeanTween.eventListeners[num] = callback;
					LeanTween.goListeners[num] = caller;
					if (LeanTween.i >= LeanTween.eventsMaxSearch)
					{
						LeanTween.eventsMaxSearch = LeanTween.i + 1;
					}
					return;
				}
				if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
				{
					return;
				}
				LeanTween.i++;
			}
			Debug.LogError("You ran out of areas to add listeners, consider increasing INIT_LISTENERS_MAX, ex: LeanTween.INIT_LISTENERS_MAX = " + LeanTween.INIT_LISTENERS_MAX * 2);
		}

		public static bool removeListener(int eventId, Action<LTEvent> callback)
		{
			return LeanTween.removeListener(LeanTween.tweenEmpty, eventId, callback);
		}

		public static bool removeListener(GameObject caller, int eventId, Action<LTEvent> callback)
		{
			LeanTween.i = 0;
			while (LeanTween.i < LeanTween.eventsMaxSearch)
			{
				int num = eventId * LeanTween.INIT_LISTENERS_MAX + LeanTween.i;
				if (LeanTween.goListeners[num] == caller && object.Equals(LeanTween.eventListeners[num], callback))
				{
					LeanTween.eventListeners[num] = null;
					LeanTween.goListeners[num] = null;
					return true;
				}
				LeanTween.i++;
			}
			return false;
		}

		public static void dispatchEvent(int eventId)
		{
			LeanTween.dispatchEvent(eventId, null);
		}

		public static void dispatchEvent(int eventId, object data)
		{
			for (int i = 0; i < LeanTween.eventsMaxSearch; i++)
			{
				int num = eventId * LeanTween.INIT_LISTENERS_MAX + i;
				if (LeanTween.eventListeners[num] != null)
				{
					if (LeanTween.goListeners[num])
					{
						LeanTween.eventListeners[num].Invoke(new LTEvent(eventId, data));
					}
					else
					{
						LeanTween.eventListeners[num] = null;
					}
				}
			}
		}
	}
}
