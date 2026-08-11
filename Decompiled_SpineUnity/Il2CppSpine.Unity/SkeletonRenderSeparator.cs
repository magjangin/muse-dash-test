using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonRenderSeparator : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_DefaultSortingOrderIncrement;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonRenderer;

	private static readonly IntPtr NativeFieldInfoPtr_mainMeshRenderer;

	private static readonly IntPtr NativeFieldInfoPtr_copyPropertyBlock;

	private static readonly IntPtr NativeFieldInfoPtr_copyMeshRendererFlags;

	private static readonly IntPtr NativeFieldInfoPtr_partsRenderers;

	private static readonly IntPtr NativeFieldInfoPtr_OnMeshAndMaterialsUpdated;

	private static readonly IntPtr NativeFieldInfoPtr_copiedBlock;

	private static readonly IntPtr NativeMethodInfoPtr_get_SkeletonRenderer_Public_get_SkeletonRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_SkeletonRenderer_Public_set_Void_SkeletonRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddToSkeletonRenderer_Public_Static_SkeletonRenderSeparator_SkeletonRenderer_Int32_Int32_Int32_Int32_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddPartsRenderer_Public_SkeletonPartsRenderer_Int32_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleRender_Private_Void_SkeletonRendererInstruction_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static int DefaultSortingOrderIncrement
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultSortingOrderIncrement, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultSortingOrderIncrement, (void*)(&num));
		}
	}

	public unsafe SkeletonRenderer skeletonRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonRenderer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer));
		}
	}

	public unsafe MeshRenderer mainMeshRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mainMeshRenderer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mainMeshRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
		}
	}

	public unsafe bool copyPropertyBlock
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_copyPropertyBlock);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_copyPropertyBlock)) = flag;
		}
	}

	public unsafe bool copyMeshRendererFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_copyMeshRendererFlags);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_copyMeshRendererFlags)) = flag;
		}
	}

	public unsafe List<SkeletonPartsRenderer> partsRenderers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_partsRenderers);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<SkeletonPartsRenderer>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_partsRenderers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe SkeletonRenderer.SkeletonRendererDelegate OnMeshAndMaterialsUpdated
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnMeshAndMaterialsUpdated);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderer.SkeletonRendererDelegate>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnMeshAndMaterialsUpdated)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRendererDelegate));
		}
	}

	public unsafe MaterialPropertyBlock copiedBlock
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_copiedBlock);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MaterialPropertyBlock>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_copiedBlock)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertyBlock));
		}
	}

	public unsafe SkeletonRenderer SkeletonRenderer
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonRenderer_Public_get_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderer>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791155, XrefRangeEnd = 791169, MetadataInitTokenRva = 46274504L, MetadataInitFlagRva = 59848138L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SkeletonRenderer_Public_set_Void_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SkeletonRenderSeparator()
	{
		Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonRenderSeparator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr);
		NativeFieldInfoPtr_DefaultSortingOrderIncrement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, "DefaultSortingOrderIncrement");
		NativeFieldInfoPtr_skeletonRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, "skeletonRenderer");
		NativeFieldInfoPtr_mainMeshRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, "mainMeshRenderer");
		NativeFieldInfoPtr_copyPropertyBlock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, "copyPropertyBlock");
		NativeFieldInfoPtr_copyMeshRendererFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, "copyMeshRendererFlags");
		NativeFieldInfoPtr_partsRenderers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, "partsRenderers");
		NativeFieldInfoPtr_OnMeshAndMaterialsUpdated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, "OnMeshAndMaterialsUpdated");
		NativeFieldInfoPtr_copiedBlock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, "copiedBlock");
		NativeMethodInfoPtr_get_SkeletonRenderer_Public_get_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664175);
		NativeMethodInfoPtr_set_SkeletonRenderer_Public_set_Void_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664176);
		NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664177);
		NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664178);
		NativeMethodInfoPtr_AddToSkeletonRenderer_Public_Static_SkeletonRenderSeparator_SkeletonRenderer_Int32_Int32_Int32_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664179);
		NativeMethodInfoPtr_AddPartsRenderer_Public_SkeletonPartsRenderer_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664180);
		NativeMethodInfoPtr_OnEnable_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664181);
		NativeMethodInfoPtr_OnDisable_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664182);
		NativeMethodInfoPtr_HandleRender_Private_Void_SkeletonRendererInstruction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664183);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr, 100664184);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791169, XrefRangeEnd = 791173, MetadataInitTokenRva = 46274440L, MetadataInitFlagRva = 59848139L)]
	public unsafe void add_OnMeshAndMaterialsUpdated(SkeletonRenderer.SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791173, XrefRangeEnd = 791177, MetadataInitTokenRva = 46274468L, MetadataInitFlagRva = 59848140L)]
	public unsafe void remove_OnMeshAndMaterialsUpdated(SkeletonRenderer.SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791177, XrefRangeEnd = 791194, MetadataInitTokenRva = 46274232L, MetadataInitFlagRva = 59848141L)]
	public unsafe static SkeletonRenderSeparator AddToSkeletonRenderer(SkeletonRenderer skeletonRenderer, int sortingLayerID = 0, int extraPartsRenderers = 0, int sortingOrderIncrement = 5, int baseSortingOrder = 0, bool addMinimumPartsRenderers = true)
	{
		IntPtr* ptr = stackalloc IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &sortingLayerID;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &extraPartsRenderers;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &sortingOrderIncrement;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &baseSortingOrder;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &addMinimumPartsRenderers;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddToSkeletonRenderer_Public_Static_SkeletonRenderSeparator_SkeletonRenderer_Int32_Int32_Int32_Int32_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderSeparator>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791194, XrefRangeEnd = 791208, MetadataInitTokenRva = 46274200L, MetadataInitFlagRva = 59848142L)]
	public unsafe SkeletonPartsRenderer AddPartsRenderer(int sortingOrderIncrement = 5, string name = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&sortingOrderIncrement);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddPartsRenderer_Public_SkeletonPartsRenderer_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonPartsRenderer>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791251, RefRangeEnd = 791252, XrefRangeStart = 791208, XrefRangeEnd = 791251, MetadataInitTokenRva = 46274352L, MetadataInitFlagRva = 59848143L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791252, XrefRangeEnd = 791274, MetadataInitTokenRva = 46274316L, MetadataInitFlagRva = 59848144L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791274, XrefRangeEnd = 791294, MetadataInitTokenRva = 46274280L, MetadataInitFlagRva = 59848145L)]
	public unsafe void HandleRender(SkeletonRendererInstruction instruction)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instruction);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleRender_Private_Void_SkeletonRendererInstruction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791294, XrefRangeEnd = 791301, MetadataInitTokenRva = 46274416L, MetadataInitFlagRva = 59848146L)]
	public unsafe SkeletonRenderSeparator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonRenderSeparator>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonRenderSeparator(IntPtr pointer)
		: base(pointer)
	{
	}
}
