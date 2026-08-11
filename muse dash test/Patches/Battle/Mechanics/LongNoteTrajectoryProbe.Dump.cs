using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 롱노트 프로브의 계층 덤프 부분입니다.
    /// <para><c>LongPressController.TryGetVisibleComponent</c>가 실패해 몸통 슬라이더
    /// (<c>LongPressNoteVisibleController.m_SliderRenderer</c>)를 못 찾았기 때문에,
    /// 실제로 어느 노드가 몸통을 그리는지(SpriteRenderer / MeshRenderer / Spine) 직접 확인하려는 목적입니다.
    /// 곡선 몸통으로 교체할 대상을 특정하는 데 씁니다.</para>
    /// </summary>
    public static partial class LongNoteTrajectoryProbe
    {
        /// <summary>계층 덤프 최대 깊이.</summary>
        private const int MaxDumpDepth = 5;

        /// <summary>계층 덤프 최대 노드 수(로그 폭발 방지).</summary>
        private const int MaxDumpNodes = 70;

        /// <summary>롱노트 한 개의 계층 전체와 각 노드의 컴포넌트를 1회 덤프합니다.</summary>
        private static void DumpHierarchyOnce(Il2Cpp.LongPressController head)
        {
            if (!EnableHierarchyDump || hierarchyDumped) return;
            hierarchyDumped = true;

            Transform noteTransform = SafeTransform(head);
            if (noteTransform == null) return;

            try
            {
                Transform top = noteTransform.parent != null ? noteTransform.parent : noteTransform;
                MelonLogger.Msg($"[LongNoteProbe] === 계층 덤프 시작: top='{top.name}' ===");
                DumpTransform(top, 0);
                MelonLogger.Msg($"[LongNoteProbe] === 계층 덤프 끝 (노드 {dumpedNodes}개) ===");
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"[LongNoteProbe] 계층 덤프 예외: {ex.GetType().Name} {ex.Message}");
            }
        }

        private static void DumpTransform(Transform t, int depth)
        {
            if (t == null || depth > MaxDumpDepth || dumpedNodes >= MaxDumpNodes) return;
            dumpedNodes++;

            string indent = new string(' ', depth * 2);
            Vector3 local = t.localPosition;
            MelonLogger.Msg(
                $"[LongNoteProbe]   {indent}{t.name} active={t.gameObject.activeSelf} " +
                $"local=({local.x:0.##},{local.y:0.##}) scale=({t.localScale.x:0.##},{t.localScale.y:0.##}) " +
                $"{DescribeComponents(t)}");

            for (int i = 0; i < t.childCount; i++)
            {
                DumpTransform(t.GetChild(i), depth + 1);
            }
        }

        /// <summary>노드에 붙은 컴포넌트 이름을 나열합니다. Renderer 계열은 enabled/정렬 순서도 같이 찍습니다.</summary>
        private static string DescribeComponents(Transform t)
        {
            try
            {
                var components = t.gameObject.GetComponents<Component>();
                if (components == null) return "[]";

                var names = new List<string>();
                foreach (var component in components)
                {
                    if (component == null) continue;

                    string name;
                    try { name = component.GetIl2CppType().Name; }
                    catch { name = "(타입 확인 실패)"; }

                    if (string.Equals(name, "Transform", StringComparison.Ordinal)) continue;

                    try
                    {
                        var renderer = component.TryCast<Renderer>();
                        if (renderer != null)
                        {
                            name += $"(enabled={renderer.enabled}, sortingOrder={renderer.sortingOrder})";
                        }
                    }
                    catch { }

                    names.Add(name);
                }

                return "[" + string.Join(", ", names) + "]";
            }
            catch (Exception ex)
            {
                return $"[컴포넌트 조회 실패: {ex.GetType().Name}]";
            }
        }
    }
}
