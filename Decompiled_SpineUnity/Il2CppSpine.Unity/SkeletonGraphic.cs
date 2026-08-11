using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Il2CppSpine.Unity;

public class SkeletonGraphic : MaskableGraphic
{
	public sealed class SkeletonRendererDelegate : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonGraphic_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonGraphic_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static SkeletonRendererDelegate()
		{
			Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "SkeletonRendererDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr, 100664101);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonGraphic_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr, 100664102);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonGraphic_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr, 100664103);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr, 100664104);
		}

		[CallerCount(6192)]
		[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SkeletonRendererDelegate(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonRendererDelegate>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(74)]
		[CachedScanResults(RefRangeStart = 171499, RefRangeEnd = 171573, XrefRangeStart = 171499, XrefRangeEnd = 171573, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Invoke(SkeletonGraphic skeletonGraphic)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonGraphic);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonGraphic_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47234, RefRangeEnd = 47236, XrefRangeStart = 47234, XrefRangeEnd = 47236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(SkeletonGraphic skeletonGraphic, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonGraphic);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonGraphic_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 45955, RefRangeEnd = 45956, XrefRangeStart = 45955, XrefRangeEnd = 45956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SkeletonRendererDelegate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator SkeletonRendererDelegate(System.Action<SkeletonGraphic> P_0)
		{
			return DelegateSupport.ConvertDelegate<SkeletonRendererDelegate>((System.Delegate)P_0);
		}

		public static SkeletonRendererDelegate operator +(SkeletonRendererDelegate P_0, SkeletonRendererDelegate P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<SkeletonRendererDelegate>();
		}

		public static SkeletonRendererDelegate operator -(SkeletonRendererDelegate P_0, SkeletonRendererDelegate P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<SkeletonRendererDelegate>();
			}
			return (SkeletonRendererDelegate)obj;
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonDataAsset;

	private static readonly System.IntPtr NativeFieldInfoPtr_initialSkinName;

	private static readonly System.IntPtr NativeFieldInfoPtr_initialFlipX;

	private static readonly System.IntPtr NativeFieldInfoPtr_initialFlipY;

	private static readonly System.IntPtr NativeFieldInfoPtr_startingAnimation;

	private static readonly System.IntPtr NativeFieldInfoPtr_startingLoop;

	private static readonly System.IntPtr NativeFieldInfoPtr_timeScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_freeze;

	private static readonly System.IntPtr NativeFieldInfoPtr_updateMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_updateWhenInvisible;

	private static readonly System.IntPtr NativeFieldInfoPtr_unscaledTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_allowMultipleCanvasRenderers;

	private static readonly System.IntPtr NativeFieldInfoPtr_canvasRenderers;

	private static readonly System.IntPtr NativeFieldInfoPtr_SeparatorPartGameObjectName;

	private static readonly System.IntPtr NativeFieldInfoPtr_separatorSlotNames;

	private static readonly System.IntPtr NativeFieldInfoPtr_separatorSlots;

	private static readonly System.IntPtr NativeFieldInfoPtr_enableSeparatorSlots;

	private static readonly System.IntPtr NativeFieldInfoPtr_separatorParts;

	private static readonly System.IntPtr NativeFieldInfoPtr_updateSeparatorPartLocation;

	private static readonly System.IntPtr NativeFieldInfoPtr_wasUpdatedAfterInit;

	private static readonly System.IntPtr NativeFieldInfoPtr_baseTexture;

	private static readonly System.IntPtr NativeFieldInfoPtr_customTextureOverride;

	private static readonly System.IntPtr NativeFieldInfoPtr_customMaterialOverride;

	private static readonly System.IntPtr NativeFieldInfoPtr_overrideTexture;

	private static readonly System.IntPtr NativeFieldInfoPtr_skeleton;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnRebuild;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnMeshAndMaterialsUpdated;

	private static readonly System.IntPtr NativeFieldInfoPtr_state;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshGenerator;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshBuffers;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentInstructions;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshes;

	private static readonly System.IntPtr NativeFieldInfoPtr_BeforeApply;

	private static readonly System.IntPtr NativeFieldInfoPtr_UpdateLocal;

	private static readonly System.IntPtr NativeFieldInfoPtr_UpdateWorld;

	private static readonly System.IntPtr NativeFieldInfoPtr_UpdateComplete;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnPostProcessVertices;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UpdateMode_Public_get_UpdateMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UpdateMode_Public_set_Void_UpdateMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SeparatorParts_Public_get_List_1_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewSkeletonGraphicGameObject_Public_Static_SkeletonGraphic_SkeletonDataAsset_Transform_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddSkeletonGraphicComponent_Public_Static_SkeletonGraphic_GameObject_SkeletonDataAsset_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CustomTextureOverride_Public_get_Dictionary_2_Texture_Texture_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CustomMaterialOverride_Public_get_Dictionary_2_Texture_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OverrideTexture_Public_get_Texture_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OverrideTexture_Public_set_Void_Texture_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_mainTexture_Public_Virtual_get_Texture_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Rebuild_Public_Virtual_Void_CanvasUpdate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Virtual_New_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateAnimationStatus_Protected_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LateUpdate_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnBecameVisible_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnBecameInvisible_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReapplySeparatorSlotNames_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Skeleton_Public_Virtual_Final_New_get_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Skeleton_Public_set_Void_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SkeletonData_Public_get_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnRebuild_Public_add_Void_SkeletonRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnRebuild_Public_rem_Void_SkeletonRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AnimationState_Public_Virtual_Final_New_get_AnimationState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MeshGenerator_Public_get_MeshGenerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLastMesh_Public_Mesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MatchRectTransformWithBounds_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MatchRectTransformSingleRenderer_Protected_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MatchRectTransformMultipleRenderers_Protected_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetRectTransformBounds_Private_Void_Bounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnPostProcessVertices_Public_add_Void_MeshGeneratorDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnPostProcessVertices_Public_rem_Void_MeshGeneratorDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TrimRenderers_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateMesh_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasMultipleSubmeshInstructions_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitMeshBuffers_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateMeshSingleCanvasRenderer_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateMeshMultipleCanvasRenderers_Protected_Void_SkeletonRendererInstruction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnsureCanvasRendererCount_Protected_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DisableUnusedCanvasRenderers_Protected_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnsureMeshesCount_Protected_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnsureSeparatorPartCount_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateSeparatorPartParents_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkeletonDataAsset skeletonDataAsset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataAsset);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataAsset)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset));
		}
	}

	public unsafe string initialSkinName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialSkinName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialSkinName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool initialFlipX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialFlipX);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialFlipX)) = flag;
		}
	}

	public unsafe bool initialFlipY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialFlipY);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initialFlipY)) = flag;
		}
	}

	public unsafe string startingAnimation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startingAnimation);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startingAnimation)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool startingLoop
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startingLoop);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startingLoop)) = flag;
		}
	}

	public unsafe float timeScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeScale)) = num;
		}
	}

	public unsafe bool freeze
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_freeze);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_freeze)) = flag;
		}
	}

	public unsafe UpdateMode updateMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateMode);
			return *(UpdateMode*)num;
		}
		set
		{
			*(UpdateMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateMode)) = updateMode;
		}
	}

	public unsafe UpdateMode updateWhenInvisible
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateWhenInvisible);
			return *(UpdateMode*)num;
		}
		set
		{
			*(UpdateMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateWhenInvisible)) = updateMode;
		}
	}

	public unsafe bool unscaledTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unscaledTime);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unscaledTime)) = flag;
		}
	}

	public unsafe bool allowMultipleCanvasRenderers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allowMultipleCanvasRenderers);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allowMultipleCanvasRenderers)) = flag;
		}
	}

	public unsafe List<CanvasRenderer> canvasRenderers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_canvasRenderers);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CanvasRenderer>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_canvasRenderers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe static string SeparatorPartGameObjectName
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SeparatorPartGameObjectName, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SeparatorPartGameObjectName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Il2CppStringArray separatorSlotNames
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorSlotNames);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorSlotNames)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe List<Slot> separatorSlots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorSlots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Slot>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorSlots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool enableSeparatorSlots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_enableSeparatorSlots);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_enableSeparatorSlots)) = flag;
		}
	}

	public unsafe List<Transform> separatorParts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorParts);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Transform>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_separatorParts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool updateSeparatorPartLocation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateSeparatorPartLocation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateSeparatorPartLocation)) = flag;
		}
	}

	public unsafe bool wasUpdatedAfterInit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasUpdatedAfterInit);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasUpdatedAfterInit)) = flag;
		}
	}

	public unsafe Texture baseTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_baseTexture);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_baseTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture));
		}
	}

	public unsafe Dictionary<Texture, Texture> customTextureOverride
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customTextureOverride);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Texture, Texture>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customTextureOverride)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<Texture, Material> customMaterialOverride
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customMaterialOverride);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Texture, Material>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customMaterialOverride)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Texture overrideTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideTexture);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture));
		}
	}

	public unsafe Skeleton skeleton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeleton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skeleton>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeleton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton));
		}
	}

	public unsafe SkeletonRendererDelegate OnRebuild
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnRebuild);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRendererDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnRebuild)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRendererDelegate));
		}
	}

	public unsafe SkeletonRendererDelegate OnMeshAndMaterialsUpdated
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnMeshAndMaterialsUpdated);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRendererDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnMeshAndMaterialsUpdated)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRendererDelegate));
		}
	}

	public unsafe AnimationState state
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_state);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_state)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationState));
		}
	}

	public unsafe MeshGenerator meshGenerator
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshGenerator);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshGenerator>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshGenerator)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshGenerator));
		}
	}

	public unsafe DoubleBuffered<MeshRendererBuffers.SmartMesh> meshBuffers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshBuffers);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DoubleBuffered<MeshRendererBuffers.SmartMesh>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshBuffers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)doubleBuffered));
		}
	}

	public unsafe SkeletonRendererInstruction currentInstructions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentInstructions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRendererInstruction>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentInstructions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRendererInstruction));
		}
	}

	public unsafe ExposedList<Mesh> meshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Mesh>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe UpdateBonesDelegate BeforeApply
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BeforeApply);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BeforeApply)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate UpdateLocal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpdateLocal);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpdateLocal)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate UpdateWorld
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpdateWorld);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpdateWorld)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate UpdateComplete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpdateComplete);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UpdateComplete)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe MeshGeneratorDelegate OnPostProcessVertices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnPostProcessVertices);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshGeneratorDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnPostProcessVertices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshGeneratorDelegate));
		}
	}

	public unsafe virtual SkeletonDataAsset SkeletonDataAsset
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
		}
	}

	public unsafe UpdateMode UpdateMode
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UpdateMode_Public_get_UpdateMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(UpdateMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UpdateMode_Public_set_Void_UpdateMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<Transform> SeparatorParts
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SeparatorParts_Public_get_List_1_Transform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Transform>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<Texture, Texture> CustomTextureOverride
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CustomTextureOverride_Public_get_Dictionary_2_Texture_Texture_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Texture, Texture>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<Texture, Material> CustomMaterialOverride
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CustomMaterialOverride_Public_get_Dictionary_2_Texture_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Texture, Material>>(intPtr) : null;
		}
	}

	public unsafe Texture OverrideTexture
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OverrideTexture_Public_get_Texture_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789982, XrefRangeEnd = 789986, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OverrideTexture_Public_set_Void_Texture_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe override Texture mainTexture
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789986, XrefRangeEnd = 789990, MetadataInitTokenRva = 46272252L, MetadataInitFlagRva = 59827234L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_mainTexture_Public_Virtual_get_Texture_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture>(intPtr) : null;
		}
	}

	public unsafe virtual Skeleton Skeleton
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Skeleton_Public_Virtual_Final_New_get_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skeleton>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Skeleton_Public_set_Void_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SkeletonData SkeletonData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonData_Public_get_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
		}
	}

	public unsafe bool IsValid
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 790046, RefRangeEnd = 790047, XrefRangeStart = 790046, XrefRangeEnd = 790046, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual AnimationState AnimationState
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 127331, RefRangeEnd = 127332, XrefRangeStart = 127331, XrefRangeEnd = 127332, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AnimationState_Public_Virtual_Final_New_get_AnimationState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState>(intPtr) : null;
		}
	}

	public unsafe MeshGenerator MeshGenerator
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MeshGenerator_Public_get_MeshGenerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshGenerator>(intPtr) : null;
		}
	}

	static SkeletonGraphic()
	{
		Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonGraphic");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonDataAsset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "skeletonDataAsset");
		NativeFieldInfoPtr_initialSkinName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "initialSkinName");
		NativeFieldInfoPtr_initialFlipX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "initialFlipX");
		NativeFieldInfoPtr_initialFlipY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "initialFlipY");
		NativeFieldInfoPtr_startingAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "startingAnimation");
		NativeFieldInfoPtr_startingLoop = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "startingLoop");
		NativeFieldInfoPtr_timeScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "timeScale");
		NativeFieldInfoPtr_freeze = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "freeze");
		NativeFieldInfoPtr_updateMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "updateMode");
		NativeFieldInfoPtr_updateWhenInvisible = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "updateWhenInvisible");
		NativeFieldInfoPtr_unscaledTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "unscaledTime");
		NativeFieldInfoPtr_allowMultipleCanvasRenderers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "allowMultipleCanvasRenderers");
		NativeFieldInfoPtr_canvasRenderers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "canvasRenderers");
		NativeFieldInfoPtr_SeparatorPartGameObjectName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "SeparatorPartGameObjectName");
		NativeFieldInfoPtr_separatorSlotNames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "separatorSlotNames");
		NativeFieldInfoPtr_separatorSlots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "separatorSlots");
		NativeFieldInfoPtr_enableSeparatorSlots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "enableSeparatorSlots");
		NativeFieldInfoPtr_separatorParts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "separatorParts");
		NativeFieldInfoPtr_updateSeparatorPartLocation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "updateSeparatorPartLocation");
		NativeFieldInfoPtr_wasUpdatedAfterInit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "wasUpdatedAfterInit");
		NativeFieldInfoPtr_baseTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "baseTexture");
		NativeFieldInfoPtr_customTextureOverride = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "customTextureOverride");
		NativeFieldInfoPtr_customMaterialOverride = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "customMaterialOverride");
		NativeFieldInfoPtr_overrideTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "overrideTexture");
		NativeFieldInfoPtr_skeleton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "skeleton");
		NativeFieldInfoPtr_OnRebuild = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "OnRebuild");
		NativeFieldInfoPtr_OnMeshAndMaterialsUpdated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "OnMeshAndMaterialsUpdated");
		NativeFieldInfoPtr_state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "state");
		NativeFieldInfoPtr_meshGenerator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "meshGenerator");
		NativeFieldInfoPtr_meshBuffers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "meshBuffers");
		NativeFieldInfoPtr_currentInstructions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "currentInstructions");
		NativeFieldInfoPtr_meshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "meshes");
		NativeFieldInfoPtr_BeforeApply = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "BeforeApply");
		NativeFieldInfoPtr_UpdateLocal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "UpdateLocal");
		NativeFieldInfoPtr_UpdateWorld = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "UpdateWorld");
		NativeFieldInfoPtr_UpdateComplete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "UpdateComplete");
		NativeFieldInfoPtr_OnPostProcessVertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, "OnPostProcessVertices");
		NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664040);
		NativeMethodInfoPtr_get_UpdateMode_Public_get_UpdateMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664041);
		NativeMethodInfoPtr_set_UpdateMode_Public_set_Void_UpdateMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664042);
		NativeMethodInfoPtr_get_SeparatorParts_Public_get_List_1_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664043);
		NativeMethodInfoPtr_NewSkeletonGraphicGameObject_Public_Static_SkeletonGraphic_SkeletonDataAsset_Transform_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664044);
		NativeMethodInfoPtr_AddSkeletonGraphicComponent_Public_Static_SkeletonGraphic_GameObject_SkeletonDataAsset_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664045);
		NativeMethodInfoPtr_get_CustomTextureOverride_Public_get_Dictionary_2_Texture_Texture_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664046);
		NativeMethodInfoPtr_get_CustomMaterialOverride_Public_get_Dictionary_2_Texture_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664047);
		NativeMethodInfoPtr_get_OverrideTexture_Public_get_Texture_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664048);
		NativeMethodInfoPtr_set_OverrideTexture_Public_set_Void_Texture_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664049);
		NativeMethodInfoPtr_get_mainTexture_Public_Virtual_get_Texture_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664050);
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664051);
		NativeMethodInfoPtr_Rebuild_Public_Virtual_Void_CanvasUpdate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664052);
		NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664053);
		NativeMethodInfoPtr_Update_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664054);
		NativeMethodInfoPtr_Update_Public_Virtual_New_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664055);
		NativeMethodInfoPtr_UpdateAnimationStatus_Protected_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664056);
		NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664057);
		NativeMethodInfoPtr_LateUpdate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664058);
		NativeMethodInfoPtr_OnBecameVisible_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664059);
		NativeMethodInfoPtr_OnBecameInvisible_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664060);
		NativeMethodInfoPtr_ReapplySeparatorSlotNames_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664061);
		NativeMethodInfoPtr_get_Skeleton_Public_Virtual_Final_New_get_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664062);
		NativeMethodInfoPtr_set_Skeleton_Public_set_Void_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664063);
		NativeMethodInfoPtr_get_SkeletonData_Public_get_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664064);
		NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664065);
		NativeMethodInfoPtr_add_OnRebuild_Public_add_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664066);
		NativeMethodInfoPtr_remove_OnRebuild_Public_rem_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664067);
		NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664068);
		NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664069);
		NativeMethodInfoPtr_get_AnimationState_Public_Virtual_Final_New_get_AnimationState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664070);
		NativeMethodInfoPtr_get_MeshGenerator_Public_get_MeshGenerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664071);
		NativeMethodInfoPtr_GetLastMesh_Public_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664072);
		NativeMethodInfoPtr_MatchRectTransformWithBounds_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664073);
		NativeMethodInfoPtr_MatchRectTransformSingleRenderer_Protected_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664074);
		NativeMethodInfoPtr_MatchRectTransformMultipleRenderers_Protected_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664075);
		NativeMethodInfoPtr_SetRectTransformBounds_Private_Void_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664076);
		NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664077);
		NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664078);
		NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664079);
		NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664080);
		NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664081);
		NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664082);
		NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664083);
		NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664084);
		NativeMethodInfoPtr_add_OnPostProcessVertices_Public_add_Void_MeshGeneratorDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664085);
		NativeMethodInfoPtr_remove_OnPostProcessVertices_Public_rem_Void_MeshGeneratorDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664086);
		NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664087);
		NativeMethodInfoPtr_TrimRenderers_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664088);
		NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664089);
		NativeMethodInfoPtr_UpdateMesh_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664090);
		NativeMethodInfoPtr_HasMultipleSubmeshInstructions_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664091);
		NativeMethodInfoPtr_InitMeshBuffers_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664092);
		NativeMethodInfoPtr_UpdateMeshSingleCanvasRenderer_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664093);
		NativeMethodInfoPtr_UpdateMeshMultipleCanvasRenderers_Protected_Void_SkeletonRendererInstruction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664094);
		NativeMethodInfoPtr_EnsureCanvasRendererCount_Protected_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664095);
		NativeMethodInfoPtr_DisableUnusedCanvasRenderers_Protected_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664096);
		NativeMethodInfoPtr_EnsureMeshesCount_Protected_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664097);
		NativeMethodInfoPtr_EnsureSeparatorPartCount_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664098);
		NativeMethodInfoPtr_UpdateSeparatorPartParents_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664099);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr, 100664100);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789956, XrefRangeEnd = 789974, MetadataInitTokenRva = 46271592L, MetadataInitFlagRva = 59827232L)]
	public unsafe static SkeletonGraphic NewSkeletonGraphicGameObject(SkeletonDataAsset skeletonDataAsset, Transform parent, Material material)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewSkeletonGraphicGameObject_Public_Static_SkeletonGraphic_SkeletonDataAsset_Transform_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonGraphic>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789974, XrefRangeEnd = 789982, MetadataInitTokenRva = 46271116L, MetadataInitFlagRva = 59827233L)]
	public unsafe static SkeletonGraphic AddSkeletonGraphicComponent(GameObject gameObject, SkeletonDataAsset skeletonDataAsset, Material material)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddSkeletonGraphicComponent_Public_Static_SkeletonGraphic_GameObject_SkeletonDataAsset_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonGraphic>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789990, XrefRangeEnd = 789992, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789992, XrefRangeEnd = 789998, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Rebuild(CanvasUpdate update)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&update);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Rebuild_Public_Virtual_Void_CanvasUpdate_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789998, XrefRangeEnd = 790009, MetadataInitTokenRva = 46271628L, MetadataInitFlagRva = 59827235L)]
	public unsafe override void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790009, XrefRangeEnd = 790012, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790012, XrefRangeEnd = 790023, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Update(float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&deltaTime);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Public_Virtual_New_Void_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790023, XrefRangeEnd = 790026, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateAnimationStatus(float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&deltaTime);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateAnimationStatus_Protected_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790026, XrefRangeEnd = 790035, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyAnimation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790036, RefRangeEnd = 790037, XrefRangeStart = 790035, XrefRangeEnd = 790036, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void OnBecameVisible()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBecameVisible_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void OnBecameInvisible()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBecameInvisible_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790037, XrefRangeEnd = 790046, MetadataInitTokenRva = 46271680L, MetadataInitFlagRva = 59827236L)]
	public unsafe void ReapplySeparatorSlotNames()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReapplySeparatorSlotNames_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790051, RefRangeEnd = 790052, XrefRangeStart = 790047, XrefRangeEnd = 790051, MetadataInitTokenRva = 46272100L, MetadataInitFlagRva = 59827237L)]
	public unsafe void add_OnRebuild(SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnRebuild_Public_add_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 790056, RefRangeEnd = 790060, XrefRangeStart = 790052, XrefRangeEnd = 790056, MetadataInitTokenRva = 46272412L, MetadataInitFlagRva = 59827238L)]
	public unsafe void remove_OnRebuild(SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnRebuild_Public_rem_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790060, XrefRangeEnd = 790064, MetadataInitTokenRva = 46271988L, MetadataInitFlagRva = 59827239L)]
	public unsafe void add_OnMeshAndMaterialsUpdated(SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790064, XrefRangeEnd = 790068, MetadataInitTokenRva = 46272340L, MetadataInitFlagRva = 59827240L)]
	public unsafe void remove_OnMeshAndMaterialsUpdated(SkeletonRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790068, XrefRangeEnd = 790071, MetadataInitTokenRva = 46271368L, MetadataInitFlagRva = 59827241L)]
	public unsafe Mesh GetLastMesh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLastMesh_Public_Mesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790071, XrefRangeEnd = 790074, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool MatchRectTransformWithBounds()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MatchRectTransformWithBounds_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790085, RefRangeEnd = 790086, XrefRangeStart = 790074, XrefRangeEnd = 790085, MetadataInitTokenRva = 46271548L, MetadataInitFlagRva = 59827242L)]
	public unsafe bool MatchRectTransformSingleRenderer()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MatchRectTransformSingleRenderer_Protected_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790100, RefRangeEnd = 790101, XrefRangeStart = 790086, XrefRangeEnd = 790100, MetadataInitTokenRva = 46271536L, MetadataInitFlagRva = 59827243L)]
	public unsafe bool MatchRectTransformMultipleRenderers()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MatchRectTransformMultipleRenderers_Protected_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 790112, RefRangeEnd = 790114, XrefRangeStart = 790101, XrefRangeEnd = 790112, MetadataInitTokenRva = 46271732L, MetadataInitFlagRva = 59827244L)]
	public unsafe void SetRectTransformBounds(Bounds combinedBounds)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&combinedBounds);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRectTransformBounds_Private_Void_Bounds_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790114, XrefRangeEnd = 790118, MetadataInitTokenRva = 46271952L, MetadataInitFlagRva = 59827245L)]
	public unsafe void add_BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790118, XrefRangeEnd = 790122, MetadataInitTokenRva = 46272292L, MetadataInitFlagRva = 59827246L)]
	public unsafe void remove_BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790122, XrefRangeEnd = 790126, MetadataInitTokenRva = 46272144L, MetadataInitFlagRva = 59827247L)]
	public unsafe virtual void add_UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790126, XrefRangeEnd = 790130, MetadataInitTokenRva = 46272484L, MetadataInitFlagRva = 59827248L)]
	public unsafe virtual void remove_UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790130, XrefRangeEnd = 790134, MetadataInitTokenRva = 46272212L, MetadataInitFlagRva = 59827249L)]
	public unsafe virtual void add_UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790134, XrefRangeEnd = 790138, MetadataInitTokenRva = 46272504L, MetadataInitFlagRva = 59827250L)]
	public unsafe virtual void remove_UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790138, XrefRangeEnd = 790142, MetadataInitTokenRva = 46272120L, MetadataInitFlagRva = 59827251L)]
	public unsafe virtual void add_UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790142, XrefRangeEnd = 790146, MetadataInitTokenRva = 46272448L, MetadataInitFlagRva = 59827252L)]
	public unsafe virtual void remove_UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790146, XrefRangeEnd = 790150, MetadataInitTokenRva = 46272048L, MetadataInitFlagRva = 59827253L)]
	public unsafe void add_OnPostProcessVertices(MeshGeneratorDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnPostProcessVertices_Public_add_Void_MeshGeneratorDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790150, XrefRangeEnd = 790154, MetadataInitTokenRva = 46272380L, MetadataInitFlagRva = 59827254L)]
	public unsafe void remove_OnPostProcessVertices(MeshGeneratorDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnPostProcessVertices_Public_rem_Void_MeshGeneratorDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790154, XrefRangeEnd = 790174, MetadataInitTokenRva = 46271164L, MetadataInitFlagRva = 59827255L)]
	public unsafe void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790174, XrefRangeEnd = 790204, MetadataInitTokenRva = 46271748L, MetadataInitFlagRva = 59827256L)]
	public unsafe void TrimRenderers()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TrimRenderers_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 790250, RefRangeEnd = 790260, XrefRangeStart = 790204, XrefRangeEnd = 790250, MetadataInitTokenRva = 46271492L, MetadataInitFlagRva = 59827257L)]
	public unsafe void Initialize(bool overwrite)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&overwrite);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 790264, RefRangeEnd = 790267, XrefRangeStart = 790260, XrefRangeEnd = 790264, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateMesh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateMesh_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790267, XrefRangeEnd = 790271, MetadataInitTokenRva = 46271400L, MetadataInitFlagRva = 59827258L)]
	public unsafe bool HasMultipleSubmeshInstructions()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasMultipleSubmeshInstructions_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790271, XrefRangeEnd = 790286, MetadataInitTokenRva = 46271448L, MetadataInitFlagRva = 59827259L)]
	public unsafe void InitMeshBuffers()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitMeshBuffers_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790330, RefRangeEnd = 790331, XrefRangeStart = 790286, XrefRangeEnd = 790330, MetadataInitTokenRva = 46271824L, MetadataInitFlagRva = 59827260L)]
	public unsafe void UpdateMeshSingleCanvasRenderer()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateMeshSingleCanvasRenderer_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790415, RefRangeEnd = 790416, XrefRangeStart = 790331, XrefRangeEnd = 790415, MetadataInitTokenRva = 46271792L, MetadataInitFlagRva = 59827261L)]
	public unsafe void UpdateMeshMultipleCanvasRenderers(SkeletonRendererInstruction currentInstructions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)currentInstructions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateMeshMultipleCanvasRenderers_Protected_Void_SkeletonRendererInstruction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790447, RefRangeEnd = 790448, XrefRangeStart = 790416, XrefRangeEnd = 790447, MetadataInitTokenRva = 46271252L, MetadataInitFlagRva = 59827262L)]
	public unsafe void EnsureCanvasRendererCount(int targetCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&targetCount);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureCanvasRendererCount_Protected_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 790455, RefRangeEnd = 790457, XrefRangeStart = 790448, XrefRangeEnd = 790455, MetadataInitTokenRva = 46271216L, MetadataInitFlagRva = 59827263L)]
	public unsafe void DisableUnusedCanvasRenderers(int usedCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&usedCount);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DisableUnusedCanvasRenderers_Protected_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790457, XrefRangeEnd = 790468, MetadataInitTokenRva = 46271288L, MetadataInitFlagRva = 59827264L)]
	public unsafe void EnsureMeshesCount(int targetCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&targetCount);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureMeshesCount_Protected_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790496, RefRangeEnd = 790497, XrefRangeStart = 790468, XrefRangeEnd = 790496, MetadataInitTokenRva = 46271312L, MetadataInitFlagRva = 59827265L)]
	public unsafe void EnsureSeparatorPartCount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureSeparatorPartCount_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 790517, RefRangeEnd = 790518, XrefRangeStart = 790497, XrefRangeEnd = 790517, MetadataInitTokenRva = 46271892L, MetadataInitFlagRva = 59827266L)]
	public unsafe void UpdateSeparatorPartParents()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSeparatorPartParents_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790518, XrefRangeEnd = 790561, MetadataInitTokenRva = 46271916L, MetadataInitFlagRva = 59827267L)]
	public unsafe SkeletonGraphic()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonGraphic>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonGraphic(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
