using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonDataAsset : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_atlasAssets;

	private static readonly IntPtr NativeFieldInfoPtr_scale;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonJSON;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonDataModifiers;

	private static readonly IntPtr NativeFieldInfoPtr_fromAnimation;

	private static readonly IntPtr NativeFieldInfoPtr_toAnimation;

	private static readonly IntPtr NativeFieldInfoPtr_duration;

	private static readonly IntPtr NativeFieldInfoPtr_defaultMix;

	private static readonly IntPtr NativeFieldInfoPtr_controller;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonData;

	private static readonly IntPtr NativeFieldInfoPtr_stateData;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsLoaded_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SkeletonDataAsset_TextAsset_AtlasAssetBase_Boolean_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SkeletonDataAsset_TextAsset_Il2CppReferenceArray_1_AtlasAssetBase_Boolean_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetAnimationStateData_Public_AnimationStateData_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetSkeletonData_Public_SkeletonData_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_InitializeWithData_Internal_Void_SkeletonData_0;

	private static readonly IntPtr NativeMethodInfoPtr_FillStateData_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetAtlasArray_Internal_Il2CppReferenceArray_1_Atlas_0;

	private static readonly IntPtr NativeMethodInfoPtr_ReadSkeletonData_Internal_Static_SkeletonData_Il2CppStructArray_1_Byte_AttachmentLoader_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ReadSkeletonData_Internal_Static_SkeletonData_String_AttachmentLoader_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<AtlasAssetBase> atlasAssets
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlasAssets);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<AtlasAssetBase>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlasAssets)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float scale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scale)) = num;
		}
	}

	public unsafe TextAsset skeletonJSON
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonJSON);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextAsset>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonJSON)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textAsset));
		}
	}

	public unsafe List<SkeletonDataModifierAsset> skeletonDataModifiers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataModifiers);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<SkeletonDataModifierAsset>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataModifiers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Il2CppStringArray fromAnimation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fromAnimation);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fromAnimation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStringArray toAnimation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_toAnimation);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_toAnimation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<float> duration
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_duration);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_duration)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float defaultMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultMix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultMix)) = num;
		}
	}

	public unsafe RuntimeAnimatorController controller
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controller);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RuntimeAnimatorController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controller)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)runtimeAnimatorController));
		}
	}

	public unsafe SkeletonData skeletonData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData));
		}
	}

	public unsafe AnimationStateData stateData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stateData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationStateData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stateData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationStateData));
		}
	}

	public unsafe bool IsLoaded
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsLoaded_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SkeletonDataAsset()
	{
		Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonDataAsset");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr);
		NativeFieldInfoPtr_atlasAssets = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "atlasAssets");
		NativeFieldInfoPtr_scale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "scale");
		NativeFieldInfoPtr_skeletonJSON = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "skeletonJSON");
		NativeFieldInfoPtr_skeletonDataModifiers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "skeletonDataModifiers");
		NativeFieldInfoPtr_fromAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "fromAnimation");
		NativeFieldInfoPtr_toAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "toAnimation");
		NativeFieldInfoPtr_duration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "duration");
		NativeFieldInfoPtr_defaultMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "defaultMix");
		NativeFieldInfoPtr_controller = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "controller");
		NativeFieldInfoPtr_skeletonData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "skeletonData");
		NativeFieldInfoPtr_stateData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, "stateData");
		NativeMethodInfoPtr_get_IsLoaded_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663874);
		NativeMethodInfoPtr_Reset_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663875);
		NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SkeletonDataAsset_TextAsset_AtlasAssetBase_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663876);
		NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SkeletonDataAsset_TextAsset_Il2CppReferenceArray_1_AtlasAssetBase_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663877);
		NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663878);
		NativeMethodInfoPtr_GetAnimationStateData_Public_AnimationStateData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663879);
		NativeMethodInfoPtr_GetSkeletonData_Public_SkeletonData_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663880);
		NativeMethodInfoPtr_InitializeWithData_Internal_Void_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663881);
		NativeMethodInfoPtr_FillStateData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663882);
		NativeMethodInfoPtr_GetAtlasArray_Internal_Il2CppReferenceArray_1_Atlas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663883);
		NativeMethodInfoPtr_ReadSkeletonData_Internal_Static_SkeletonData_Il2CppStructArray_1_Byte_AttachmentLoader_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663884);
		NativeMethodInfoPtr_ReadSkeletonData_Internal_Static_SkeletonData_String_AttachmentLoader_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663885);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr, 100663886);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788316, XrefRangeEnd = 788329, MetadataInitTokenRva = 46269992L, MetadataInitFlagRva = 59827222L)]
	public unsafe static SkeletonDataAsset CreateRuntimeInstance(TextAsset skeletonDataFile, AtlasAssetBase atlasAsset, bool initialize, float scale = 0.01f)
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataFile);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasAsset);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &initialize;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &scale;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SkeletonDataAsset_TextAsset_AtlasAssetBase_Boolean_Single_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788329, XrefRangeEnd = 788337, MetadataInitTokenRva = 46269952L, MetadataInitFlagRva = 59827223L)]
	public unsafe static SkeletonDataAsset CreateRuntimeInstance(TextAsset skeletonDataFile, Il2CppReferenceArray<AtlasAssetBase> atlasAssets, bool initialize, float scale = 0.01f)
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataFile);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasAssets);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &initialize;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &scale;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SkeletonDataAsset_TextAsset_Il2CppReferenceArray_1_AtlasAssetBase_Boolean_Single_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 788338, RefRangeEnd = 788340, XrefRangeStart = 788337, XrefRangeEnd = 788338, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AnimationStateData GetAnimationStateData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAnimationStateData_Public_AnimationStateData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationStateData>(intPtr) : null;
	}

	[CallerCount(15)]
	[CachedScanResults(RefRangeStart = 788434, RefRangeEnd = 788449, XrefRangeStart = 788340, XrefRangeEnd = 788434, MetadataInitTokenRva = 46270068L, MetadataInitFlagRva = 59827224L)]
	public unsafe SkeletonData GetSkeletonData(bool quiet)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&quiet);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSkeletonData_Public_SkeletonData_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788449, XrefRangeEnd = 788456, MetadataInitTokenRva = 46270124L, MetadataInitFlagRva = 59827225L)]
	public unsafe void InitializeWithData(SkeletonData sd)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sd);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitializeWithData_Internal_Void_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788456, XrefRangeEnd = 788457, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FillStateData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FillStateData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788457, XrefRangeEnd = 788473, MetadataInitTokenRva = 46270032L, MetadataInitFlagRva = 59827226L)]
	public unsafe Il2CppReferenceArray<Atlas> GetAtlasArray()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAtlasArray_Internal_Il2CppReferenceArray_1_Atlas_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Atlas>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788484, RefRangeEnd = 788485, XrefRangeStart = 788473, XrefRangeEnd = 788484, MetadataInitTokenRva = 46270208L, MetadataInitFlagRva = 59827227L)]
	public unsafe static SkeletonData ReadSkeletonData(Il2CppStructArray<byte> bytes, AttachmentLoader attachmentLoader, float scale)
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bytes);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachmentLoader);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &scale;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadSkeletonData_Internal_Static_SkeletonData_Il2CppStructArray_1_Byte_AttachmentLoader_Single_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788485, XrefRangeEnd = 788494, MetadataInitTokenRva = 46270168L, MetadataInitFlagRva = 59827228L)]
	public unsafe static SkeletonData ReadSkeletonData(string text, AttachmentLoader attachmentLoader, float scale)
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(text);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachmentLoader);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &scale;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadSkeletonData_Internal_Static_SkeletonData_String_AttachmentLoader_Single_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788494, XrefRangeEnd = 788513, MetadataInitTokenRva = 46270252L, MetadataInitFlagRva = 59827229L)]
	public unsafe SkeletonDataAsset()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonDataAsset>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonDataAsset(IntPtr pointer)
		: base(pointer)
	{
	}
}
