using Common.Enum;
using DG.Tweening;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIUtil : MonoBehaviour
{
	public static void AnimationOnFocus(Transform target, Action onFinished)
	{
		target.get_transform().set_localScale(new Vector3(1.1f, 1.1f, 1.1f));
		TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOScale(target, new Vector3(1f, 1f, 1f), 0.1f), delegate
		{
			if (onFinished != null)
			{
				onFinished.Invoke();
			}
		});
	}

	public static BattleFormationKinds1[] GenerateFormationArray(DeckModel deck)
	{
		HashSet<BattleFormationKinds1> selectableFormations = DeckUtil.GetSelectableFormations(deck);
		List<BattleFormationKinds1> list = new List<BattleFormationKinds1>();
		if (selectableFormations.Contains(BattleFormationKinds1.FukuJuu))
		{
			list.Add(BattleFormationKinds1.FukuJuu);
		}
		if (selectableFormations.Contains(BattleFormationKinds1.Rinkei))
		{
			list.Add(BattleFormationKinds1.Rinkei);
		}
		if (selectableFormations.Contains(BattleFormationKinds1.TanJuu))
		{
			list.Add(BattleFormationKinds1.TanJuu);
		}
		if (selectableFormations.Contains(BattleFormationKinds1.TanOu))
		{
			list.Add(BattleFormationKinds1.TanOu);
		}
		if (selectableFormations.Contains(BattleFormationKinds1.Teikei))
		{
			list.Add(BattleFormationKinds1.Teikei);
		}
		return list.ToArray();
	}
}
