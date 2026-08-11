using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppSpine.Unity.AnimationTools;

public static class TimelineExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Evaluate_Public_Static_Vector2_TranslateTimeline_Single_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindTranslateTimelineForBone_Public_Static_TranslateTimeline_Animation_Int32_0;

	static TimelineExtensions()
	{
		Il2CppClassPointerStore<TimelineExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity.AnimationTools", "TimelineExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TimelineExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_Evaluate_Public_Static_Vector2_TranslateTimeline_Single_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TimelineExtensions>.NativeClassPtr, 100664401);
		NativeMethodInfoPtr_FindTranslateTimelineForBone_Public_Static_TranslateTimeline_Animation_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TimelineExtensions>.NativeClassPtr, 100664402);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 793465, RefRangeEnd = 793477, XrefRangeStart = 793456, XrefRangeEnd = 793465, MetadataInitTokenRva = 47169432L, MetadataInitFlagRva = 59827091L)]
	public unsafe static Vector2 Evaluate(this TranslateTimeline timeline, float time, SkeletonData skeletonData = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)timeline);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Evaluate_Public_Static_Vector2_TranslateTimeline_Single_SkeletonData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793493, RefRangeEnd = 793494, XrefRangeStart = 793477, XrefRangeEnd = 793493, MetadataInitTokenRva = 47169448L, MetadataInitFlagRva = 59827092L)]
	public unsafe static TranslateTimeline FindTranslateTimelineForBone(this Animation a, int boneIndex)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &boneIndex;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindTranslateTimelineForBone_Public_Static_TranslateTimeline_Animation_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TranslateTimeline>(intPtr) : null;
	}

	public TimelineExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
