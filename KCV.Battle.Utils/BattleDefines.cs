using Common.Enum;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Utils
{
	public class BattleDefines
	{
		public const float DECIDE_BTN_NOTIFIED_TIME = 0.3f;

		public const float FIELDCAMERA_DEFAULT_SHIP_DISTANCE = 30f;

		public const float PREFAB_GENERATION_TIME = 0.1f;

		public const float DELAY_DISCARD_TIME = 0.15f;

		public const float FORMATION_BRIGHT_POINT_INTERVAL = 10f;

		public const float FORMATION_INTERVAL_3DVIEW = 1f;

		public const float FORMATION_ADVANCED_OFFS = 5f;

		public const float FIELDCAMERA_DEFAULT_HEIGHT = 4f;

		public const float FIELDDIMCAM_DIM_AMOUNT = 0.75f;

		public const int CENTERLINE_SLOTITEM_MAX_NUM = 3;

		public const float BOSSINSERT_START_BOSS_SCALE = 0.95f;

		public const float FLEET_ADVENT_ROTATE_SPD = -10f;

		public const float FLEET_ADVENT_CLOSEUP_DIST = 30f;

		public const float FLEET_ADVENT_CLOUD_POSY = 20f;

		public const float FLEET_ADVENT_CLOSEUP_TIME = 1f;

		public const float FLEET_ADVENT_CLOSEUP_RATE = 0.95f;

		public const float DETECTION_RISE_CAM_HEIGHT = 50f;

		public const float DETECTION_RISE_CAM_MOVE_Z_RATE = 6f;

		public const float DETECTION_RISE_CAM_TIME = 1.95f;

		public const LeanTweenType DETECTION_RISE_CAM_EASING = LeanTweenType.easeInOutCubic;

		public const float DETECTION_RISE_CAM_CLOUD_OCCURS_HEIGHT_RATE = 0.15f;

		public const int DETECTION_CUTIN_DRAW_AIRCRAFT_MAX = 3;

		public const float DETECTION_ENEMY_FLEET_FOCUS_MOVE_TIME = 2.7f;

		public const float DETECTION_ENEMY_FLEET_FOCUS_MOVE_DISTANCE_RATE = 0.3f;

		public const float DETECTION_ENEMY_FLEET_FOCUS_DISTANCE_CLOSEUP_RATE = 0.7f;

		public const float COMMAND_FORMATION_INVERVAL_3DVIEW = 4f;

		public const int MAX_AIRCRAFT_COUNT = 3;

		public const float SHELLING_ATTACK_CAMERA_TO_TARGET_DICTANCE = 50f;

		public const float SHELLING_ATTACK_CAMERA_CLOSEUP_DISTANCE = 10f;

		public const float SHELLING_ATTACK_PLAY_SLOT_ANIM_DELAY_TIME = 0.033f;

		public const float SHELLING_ATTACK_CLOSE_UP_RATE = 0.98f;

		public const float SHELLING_ATTACK_MOTIONBLUR_AMOUNT = 0.65f;

		public const float SHELLING_ATTACK_DAMATE_EXPLODE_POS_RATE = 0.9f;

		public const float SHELLING_ATTACK_ATTACKER_CLOSEUP_SECOND_TIME_AIRCRAFT = 1.2f;

		public const float SHELLING_ATTACK_PLAY_SHIP_SHELLING_ANIM_DELAY_TIME = 0.4f;

		public const float SHELLING_ATTACK_CAMERA_ROTATE_TIME = 0.666f;

		public const float SHELLING_ATTACK_CAMERA_ROTATE_MOVE_TIME = 1.1655f;

		public const float SHELLING_ATTACK_CAMERA_ROTATE_POS_RATE = 0.2f;

		public const LeanTweenType SHELLING_ATTACK_CAMERA_ROTATE_LOOK_EASING_TYPE = LeanTweenType.easeInQuad;

		public const float SHELLING_ATTACK_CAMERA_ROTATE_DELAY_TIME = 0.3f;

		public const float SHELLING_PLAY_SHIP_DEFENDER_ANIM_DELAY_TIME = 0.5f;

		public const float SHELLING_ATTACK_PROTECT_FROM_SECOND_CLOSEUP_TIME = 0.425f;

		public const float SHELLING_ATTACK_PROTECT_PROTECTOR_POS_RATE = 0.58f;

		public const float SHELLING_DEPTH_CHARGE_PLAY_DEPTH_CHARGE_DELAY_TIME = 0f;

		public const float TORPEDO_STRAIGHT_DURATION = 2.65f;

		public const float DEATHCRY_NEXTPHASE_DELAY_TIME = 1f;

		public const float RESULT_VETERANSREPORT_FLEET_NUM = 2f;

		public const float RESULT_VETERANSREPORT_BANNER_SLOTIN_INTERVAL_TIME = 0.05f;

		public const float RESULT_VETERANSREPORT_BANNER_INFOIN_INTERVAL_TIME = 0.05f;

		public const float RESULT_VETERANSREPORT_SLOTIN_ALPHA_TIME = 0.5f;

		public const float RESULT_VETERANSREPORT_WARVETERANS_GAUGE_DRAW_TIME = 0.5f;

		public const float RESULT_VETERANSREPORT_WARVETERANS_GAUGE_UPDATE_TIME = 1f;

		public const float RESULT_VETERANSREPORT_BONUS_AND_MVPSHIP_SHOW_TIME = 0.5f;

		public const float RESULT_VETERANSREPORT_EXP_UPDATE_TIME = 2f;

		public const string FLAGSHIP_WRECK_DECK_FLAGSHIP_MESSAGE = "『{0}』艦隊<br>旗艦「{1}」が<br>大破しました。";

		public const string FLAGSHIP_WRECK_FLEET_HOMING_MESSAGE = "進撃は困難です……帰投します。";

		public const float FLAGSHIP_WRECK_MESSAGE_INTERVAL = 0.05f;

		public const int DAMAGE_CUTIN_DRAW_SHIP_MAX = 3;

		public static readonly Dictionary<BattleFormationKinds1, Dictionary<int, Vector3[]>> FORMATION_POSITION;

		public static readonly Dictionary<BattleFormationKinds1, Dictionary<int, Vector3[]>> FORMATION_COMBINEDFLEET_POSITION;

		public static readonly List<Vector3> BATTLESHIP_TORPEDOSALVO_WAKE_ANGLE;

		public static readonly List<Vector3> FLEET_ADVENT_START_CAM_POS;

		public static readonly LeanTweenType FLEET_ADVENT_FLEET_CLOSEUP_EASEING_TYPE;

		public static readonly Dictionary<DetectionProductionType, List<float>> DETECTION_RESULT_LABEL_POS;

		public static readonly Vector3[] AERIAL_BOMB_CAM_POSITION;

		public static readonly Quaternion[] AERIAL_BOMB_CAM_ROTATION;

		public static readonly Vector3[] AERIAL_BOMB_TRANS_ANGLE;

		public static readonly Vector4[] AERIAL_TORPEDO_WAVESPEED;

		public static readonly Vector3[] AERIAL_TORPEDO_CAM_POSITION;

		public static readonly Quaternion[] AERIAL_TORPEDO_CAM_ROTATION;

		public static readonly Vector3[] AERIAL_FRIEND_TORPEDO_POS;

		public static readonly Vector3[] AERIAL_ENEMY_TORPEDO_POS;

		public static readonly List<Vector3> SHELLING_FORMATION_JUDGE_RESULTLABEL_POS;

		public static readonly List<float> SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME;

		public static readonly List<LeanTweenType> SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE;

		public static readonly List<float> SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME;

		public static readonly List<LeanTweenType> SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE;

		public static readonly List<float> SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE;

		public static readonly List<string> RESULT_WINRUNK_JUDGE_TEXT;

		public static SoundKeep SOUND_KEEP;

		public static readonly Dictionary<int, List<Vector3>> DAMAGE_CUT_IN_SHIP_DRAW_POS;

		static BattleDefines()
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(Vector3.get_up() * -71f);
			list.Add(Vector3.get_up() * 63f);
			BattleDefines.BATTLESHIP_TORPEDOSALVO_WAKE_ANGLE = list;
			list = new List<Vector3>();
			list.Add(new Vector3(-56f, 47.5f, 6.5f));
			list.Add(new Vector3(40f, 30f, -30f));
			BattleDefines.FLEET_ADVENT_START_CAM_POS = list;
			BattleDefines.FLEET_ADVENT_FLEET_CLOSEUP_EASEING_TYPE = LeanTweenType.linear;
			Dictionary<DetectionProductionType, List<float>> dictionary = new Dictionary<DetectionProductionType, List<float>>();
			Dictionary<DetectionProductionType, List<float>> arg_D8_0 = dictionary;
			DetectionProductionType arg_D8_1 = DetectionProductionType.Succes;
			List<float> list2 = new List<float>();
			list2.Add(-250f);
			list2.Add(-150f);
			list2.Add(-50f);
			list2.Add(100f);
			list2.Add(200f);
			list2.Add(275f);
			list2.Add(9999f);
			arg_D8_0.Add(arg_D8_1, list2);
			Dictionary<DetectionProductionType, List<float>> arg_133_0 = dictionary;
			DetectionProductionType arg_133_1 = DetectionProductionType.SuccesLost;
			list2 = new List<float>();
			list2.Add(-250f);
			list2.Add(-150f);
			list2.Add(-50f);
			list2.Add(100f);
			list2.Add(200f);
			list2.Add(275f);
			list2.Add(9999f);
			arg_133_0.Add(arg_133_1, list2);
			Dictionary<DetectionProductionType, List<float>> arg_18E_0 = dictionary;
			DetectionProductionType arg_18E_1 = DetectionProductionType.Lost;
			list2 = new List<float>();
			list2.Add(-250f);
			list2.Add(-150f);
			list2.Add(-50f);
			list2.Add(50f);
			list2.Add(150f);
			list2.Add(250f);
			list2.Add(350f);
			arg_18E_0.Add(arg_18E_1, list2);
			Dictionary<DetectionProductionType, List<float>> arg_1E9_0 = dictionary;
			DetectionProductionType arg_1E9_1 = DetectionProductionType.NotFound;
			list2 = new List<float>();
			list2.Add(-165f);
			list2.Add(-65f);
			list2.Add(65f);
			list2.Add(165f);
			list2.Add(250f);
			list2.Add(9999f);
			list2.Add(9999f);
			arg_1E9_0.Add(arg_1E9_1, list2);
			BattleDefines.DETECTION_RESULT_LABEL_POS = dictionary;
			BattleDefines.AERIAL_BOMB_CAM_POSITION = new Vector3[]
			{
				new Vector3(20f, 15f, 0f),
				new Vector3(20f, 15f, 0f)
			};
			BattleDefines.AERIAL_BOMB_CAM_ROTATION = new Quaternion[]
			{
				Quaternion.Euler(new Vector3(-16f, 90f, 0f)),
				Quaternion.Euler(new Vector3(-16f, -90f, 0f))
			};
			BattleDefines.AERIAL_BOMB_TRANS_ANGLE = new Vector3[]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, -180f, 0f)
			};
			BattleDefines.AERIAL_TORPEDO_WAVESPEED = new Vector4[]
			{
				new Vector4(-4f, -2000f, 5f, -1600f),
				new Vector4(-4f, 2000f, 5f, 1600f)
			};
			BattleDefines.AERIAL_TORPEDO_CAM_POSITION = new Vector3[]
			{
				new Vector3(-21.3f, 6.2f, -7f),
				new Vector3(-23f, 6.2f, 5.5f)
			};
			BattleDefines.AERIAL_TORPEDO_CAM_ROTATION = new Quaternion[]
			{
				Quaternion.Euler(new Vector3(16.29f, 90f, 0f)),
				Quaternion.Euler(new Vector3(16f, 90f, 0f))
			};
			BattleDefines.AERIAL_FRIEND_TORPEDO_POS = new Vector3[]
			{
				new Vector3(-9f, 0f, -2.5f),
				new Vector3(0f, 0f, -5f),
				new Vector3(-9f, 0f, -7f)
			};
			BattleDefines.AERIAL_ENEMY_TORPEDO_POS = new Vector3[]
			{
				new Vector3(-11f, 0f, 1f),
				new Vector3(-2f, 0f, -1f),
				new Vector3(-11f, 0f, -2.5f)
			};
			list = new List<Vector3>();
			list.Add(Vector3.get_up() * 30f);
			list.Add(Vector3.get_down() * 90f);
			BattleDefines.SHELLING_FORMATION_JUDGE_RESULTLABEL_POS = list;
			list2 = new List<float>();
			list2.Add(0.334f);
			list2.Add(1f);
			BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME = list2;
			List<LeanTweenType> list3 = new List<LeanTweenType>();
			list3.Add(LeanTweenType.easeInSine);
			list3.Add(LeanTweenType.linear);
			BattleDefines.SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE = list3;
			list2 = new List<float>();
			list2.Add(0.3f);
			list2.Add(1.3f);
			list2.Add(3.504f);
			BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME = list2;
			list3 = new List<LeanTweenType>();
			list3.Add(LeanTweenType.linear);
			list3.Add(LeanTweenType.linear);
			BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE = list3;
			list2 = new List<float>();
			list2.Add(0.85f);
			list2.Add(0.845f);
			BattleDefines.SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE = list2;
			List<string> list4 = new List<string>();
			list4.Add(string.Empty);
			list4.Add("敗北");
			list4.Add("敗北");
			list4.Add("戦術的敗北!!");
			list4.Add("戦術的勝利!!");
			list4.Add("勝利!!");
			list4.Add("勝利!!");
			list4.Add("完全勝利!!");
			BattleDefines.RESULT_WINRUNK_JUDGE_TEXT = list4;
			BattleDefines.SOUND_KEEP = default(SoundKeep);
			Dictionary<int, List<Vector3>> dictionary2 = new Dictionary<int, List<Vector3>>();
			Dictionary<int, List<Vector3>> arg_649_0 = dictionary2;
			int arg_649_1 = 1;
			list = new List<Vector3>();
			list.Add(Vector3.get_zero());
			list.Add(Vector3.get_right() * 2000f);
			list.Add(Vector3.get_right() * 2000f);
			arg_649_0.Add(arg_649_1, list);
			Dictionary<int, List<Vector3>> arg_6A1_0 = dictionary2;
			int arg_6A1_1 = 2;
			list = new List<Vector3>();
			list.Add(new Vector3(-200f, 0f, 0f));
			list.Add(Vector3.get_right() * 2000f);
			list.Add(new Vector3(200f, 0f, 0f));
			arg_6A1_0.Add(arg_6A1_1, list);
			Dictionary<int, List<Vector3>> arg_6EF_0 = dictionary2;
			int arg_6EF_1 = 3;
			list = new List<Vector3>();
			list.Add(new Vector3(-300f, 0f, 0f));
			list.Add(new Vector3(300f, 0f, 0f));
			list.Add(Vector3.get_zero());
			arg_6EF_0.Add(arg_6EF_1, list);
			BattleDefines.DAMAGE_CUT_IN_SHIP_DRAW_POS = dictionary2;
			BattleDefines.FORMATION_POSITION = new Dictionary<BattleFormationKinds1, Dictionary<int, Vector3[]>>();
			BattleDefines.FORMATION_COMBINEDFLEET_POSITION = new Dictionary<BattleFormationKinds1, Dictionary<int, Vector3[]>>();
			BattleDefines.CalcFormationBrightPoint();
		}

		private static void CalcFormationBrightPoint()
		{
			float num = 10f;
			using (IEnumerator enumerator = Enum.GetValues(typeof(BattleFormationKinds1)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch ((int)enumerator.get_Current())
					{
					case 1:
					{
						Dictionary<int, Vector3[]> dictionary = new Dictionary<int, Vector3[]>();
						for (int i = 1; i <= 6; i++)
						{
							Vector3[] array = Util.CalcNWayPosZ(Vector3.get_zero(), i, num);
							dictionary.Add(i, array);
						}
						BattleDefines.FORMATION_POSITION.Add(BattleFormationKinds1.TanJuu, dictionary);
						break;
					}
					case 2:
					{
						Dictionary<int, Vector3[]> dictionary2 = new Dictionary<int, Vector3[]>();
						for (int j = 1; j <= 6; j++)
						{
							if (j <= 2)
							{
								Vector3[] array2 = Util.CalcNWayPosX(Vector3.get_zero(), j, num);
								dictionary2.Add(j, array2);
							}
							else if (j <= 4)
							{
								Vector3[] array3 = Util.CalcNWayPosX(Vector3.get_zero(), 2, num);
								Vector3[] array2 = new Vector3[j];
								for (int k = 0; k < array2.Length; k++)
								{
									float num2 = (k >= 2) ? num : (-num);
									float z = (k % 2 != 0) ? array3[1].x : array3[0].x;
									array2[k].x = num2 / 2f;
									array2[k].y = array3[0].y;
									array2[k].z = z;
								}
								dictionary2.Add(j, array2);
							}
							else
							{
								Vector3[] array4 = Util.CalcNWayPosX(Vector3.get_zero(), 3, num);
								Vector3[] array2 = new Vector3[j];
								for (int l = 0; l < array2.Length; l++)
								{
									float num3 = (l >= 3) ? num : (-num);
									array2[l].x = num3 / 2f;
									array2[l].y = array4[0].y;
									array2[l].z = array4[l % 3].x;
								}
								dictionary2.Add(j, array2);
							}
						}
						BattleDefines.FORMATION_POSITION.Add(BattleFormationKinds1.FukuJuu, dictionary2);
						break;
					}
					case 3:
					{
						Dictionary<int, Vector3[]> dictionary3 = new Dictionary<int, Vector3[]>();
						for (int m = 1; m <= 6; m++)
						{
							Vector3[] array5;
							if (m <= 2)
							{
								array5 = Util.CalcNWayPosZ(Vector3.get_zero(), m, num);
							}
							else if (m <= 5)
							{
								int verNum = (m != 3) ? (m - 1) : m;
								Vector2[] array6 = Mathe.RegularPolygonVertices(verNum, num / 2f, -90f);
								array5 = new Vector3[m];
								if (m == 3)
								{
									int num4 = 0;
									Vector2[] array7 = array6;
									for (int n = 0; n < array7.Length; n++)
									{
										Vector2 vector = array7[n];
										array5[num4].x = array6[num4].x;
										array5[num4].y = 0f;
										array5[num4].z = array6[num4].y;
										num4++;
									}
								}
								else
								{
									array5[0] = Vector3.get_zero();
									int num4 = 0;
									Vector2[] array8 = array6;
									for (int num5 = 0; num5 < array8.Length; num5++)
									{
										Vector2 vector2 = array8[num5];
										array5[num4 + 1].x = array6[num4].x;
										array5[num4 + 1].y = 0f;
										array5[num4 + 1].z = array6[num4].y;
										num4++;
									}
								}
							}
							else
							{
								Vector3[] array9 = Util.CalcNWayPosZ(Vector3.get_zero(), 4, num);
								Vector3[] array10 = Util.CalcNWayPosX(Vector3.get_zero(), 3, num);
								array5 = new Vector3[m];
								array5[0] = array9[1];
								array5[1] = array9[2];
								array5[2] = array9[3];
								array5[3] = array9[0];
								array5[4] = array10[0];
								array5[5] = array10[2];
							}
							dictionary3.Add(m, array5);
						}
						BattleDefines.FORMATION_POSITION.Add(BattleFormationKinds1.Rinkei, dictionary3);
						break;
					}
					case 4:
					{
						Dictionary<int, Vector3[]> dictionary4 = new Dictionary<int, Vector3[]>();
						for (int num6 = 1; num6 <= 6; num6++)
						{
							Vector3[] array11 = Util.CalcNWayPosX(Vector3.get_zero(), num6, num / 2f);
							Vector3[] array12 = new Vector3[array11.Length];
							int num7 = 0;
							Vector3[] array13 = array11;
							for (int num8 = 0; num8 < array13.Length; num8++)
							{
								Vector3 vector3 = array13[num8];
								array12[num7].x = vector3.x;
								array12[num7].y = vector3.y;
								array12[num7].z = vector3.x;
								num7++;
							}
							dictionary4.Add(num6, array12);
						}
						BattleDefines.FORMATION_POSITION.Add(BattleFormationKinds1.Teikei, dictionary4);
						break;
					}
					case 5:
					{
						Dictionary<int, Vector3[]> dictionary5 = new Dictionary<int, Vector3[]>();
						for (int num9 = 1; num9 <= 6; num9++)
						{
							Vector3[] array14 = Util.CalcNWayPosX(Vector3.get_zero(), num9, num);
							for (int num10 = 0; num10 < array14.Length; num10++)
							{
								Vector3 vector4 = array14[num10];
								array14[num10].x = -vector4.x;
								array14[num10].y = vector4.y;
								array14[num10].z = vector4.z;
							}
							dictionary5.Add(num9, array14);
						}
						BattleDefines.FORMATION_POSITION.Add(BattleFormationKinds1.TanOu, dictionary5);
						break;
					}
					}
				}
			}
		}
	}
}
