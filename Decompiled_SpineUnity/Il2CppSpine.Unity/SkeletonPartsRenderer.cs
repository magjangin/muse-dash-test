using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonPartsRenderer : MonoBehaviour
{
	public sealed class SkeletonPartsRendererDelegate : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonPartsRenderer_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonPartsRenderer_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static SkeletonPartsRendererDelegate()
		{
			Il2CppClassPointerStore<SkeletonPartsRendererDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, "SkeletonPartsRendererDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRendererDelegate>.NativeClassPtr, 100664171);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonPartsRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRendererDelegate>.NativeClassPtr, 100664172);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonPartsRenderer_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRendererDelegate>.NativeClassPtr, 100664173);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRendererDelegate>.NativeClassPtr, 100664174);
		}

		[CallerCount(6192)]
		[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SkeletonPartsRendererDelegate(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonPartsRendererDelegate>.NativeClassPtr))
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
		public unsafe virtual void Invoke(SkeletonPartsRenderer skeletonPartsRenderer)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonPartsRenderer);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_SkeletonPartsRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47234, RefRangeEnd = 47236, XrefRangeStart = 47234, XrefRangeEnd = 47236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(SkeletonPartsRenderer skeletonPartsRenderer, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonPartsRenderer);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_SkeletonPartsRenderer_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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

		public SkeletonPartsRendererDelegate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator SkeletonPartsRendererDelegate(System.Action<SkeletonPartsRenderer> P_0)
		{
			return DelegateSupport.ConvertDelegate<SkeletonPartsRendererDelegate>((System.Delegate)P_0);
		}

		public static SkeletonPartsRendererDelegate operator +(SkeletonPartsRendererDelegate P_0, SkeletonPartsRendererDelegate P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<SkeletonPartsRendererDelegate>();
		}

		public static SkeletonPartsRendererDelegate operator -(SkeletonPartsRendererDelegate P_0, SkeletonPartsRendererDelegate P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<SkeletonPartsRendererDelegate>();
			}
			return (SkeletonPartsRendererDelegate)obj;
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_meshGenerator;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshRenderer;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshFilter;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnMeshAndMaterialsUpdated;

	private static readonly System.IntPtr NativeFieldInfoPtr_buffers;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentInstructions;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MeshGenerator_Public_get_MeshGenerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MeshRenderer_Public_get_MeshRenderer_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MeshFilter_Public_get_MeshFilter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonPartsRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonPartsRendererDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LazyIntialize_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearMesh_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RenderParts_Public_Void_ExposedList_1_SubmeshInstruction_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPropertyBlock_Public_Void_MaterialPropertyBlock_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewPartsRendererGameObject_Public_Static_SkeletonPartsRenderer_Transform_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

	public unsafe MeshRenderer meshRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
		}
	}

	public unsafe MeshFilter meshFilter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshFilter);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshFilter>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshFilter)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshFilter));
		}
	}

	public unsafe SkeletonPartsRendererDelegate OnMeshAndMaterialsUpdated
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnMeshAndMaterialsUpdated);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonPartsRendererDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnMeshAndMaterialsUpdated)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonPartsRendererDelegate));
		}
	}

	public unsafe MeshRendererBuffers buffers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buffers);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshRendererBuffers>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buffers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRendererBuffers));
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

	public unsafe MeshGenerator MeshGenerator
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791053, XrefRangeEnd = 791054, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public unsafe MeshRenderer MeshRenderer
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791054, XrefRangeEnd = 791055, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MeshRenderer_Public_get_MeshRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
	}

	public unsafe MeshFilter MeshFilter
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791055, XrefRangeEnd = 791056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MeshFilter_Public_get_MeshFilter_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshFilter>(intPtr) : null;
		}
	}

	static SkeletonPartsRenderer()
	{
		Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonPartsRenderer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr);
		NativeFieldInfoPtr_meshGenerator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, "meshGenerator");
		NativeFieldInfoPtr_meshRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, "meshRenderer");
		NativeFieldInfoPtr_meshFilter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, "meshFilter");
		NativeFieldInfoPtr_OnMeshAndMaterialsUpdated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, "OnMeshAndMaterialsUpdated");
		NativeFieldInfoPtr_buffers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, "buffers");
		NativeFieldInfoPtr_currentInstructions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, "currentInstructions");
		NativeMethodInfoPtr_get_MeshGenerator_Public_get_MeshGenerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664160);
		NativeMethodInfoPtr_get_MeshRenderer_Public_get_MeshRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664161);
		NativeMethodInfoPtr_get_MeshFilter_Public_get_MeshFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664162);
		NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonPartsRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664163);
		NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonPartsRendererDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664164);
		NativeMethodInfoPtr_LazyIntialize_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664165);
		NativeMethodInfoPtr_ClearMesh_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664166);
		NativeMethodInfoPtr_RenderParts_Public_Void_ExposedList_1_SubmeshInstruction_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664167);
		NativeMethodInfoPtr_SetPropertyBlock_Public_Void_MaterialPropertyBlock_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664168);
		NativeMethodInfoPtr_NewPartsRendererGameObject_Public_Static_SkeletonPartsRenderer_Transform_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664169);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr, 100664170);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791056, XrefRangeEnd = 791060, MetadataInitTokenRva = 46274076L, MetadataInitFlagRva = 59848133L)]
	public unsafe void add_OnMeshAndMaterialsUpdated(SkeletonPartsRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnMeshAndMaterialsUpdated_Public_add_Void_SkeletonPartsRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791060, XrefRangeEnd = 791064, MetadataInitTokenRva = 46274104L, MetadataInitFlagRva = 59848134L)]
	public unsafe void remove_OnMeshAndMaterialsUpdated(SkeletonPartsRendererDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnMeshAndMaterialsUpdated_Public_rem_Void_SkeletonPartsRendererDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14)]
	[CachedScanResults(RefRangeStart = 791081, RefRangeEnd = 791095, XrefRangeStart = 791064, XrefRangeEnd = 791081, MetadataInitTokenRva = 46273972L, MetadataInitFlagRva = 59848135L)]
	public unsafe void LazyIntialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LazyIntialize_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791095, XrefRangeEnd = 791098, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearMesh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearMesh_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791120, RefRangeEnd = 791121, XrefRangeStart = 791098, XrefRangeEnd = 791120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RenderParts(ExposedList<SubmeshInstruction> instructions, int startSubmesh, int endSubmesh)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instructions);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startSubmesh;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &endSubmesh;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RenderParts_Public_Void_ExposedList_1_SubmeshInstruction_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791124, RefRangeEnd = 791125, XrefRangeStart = 791121, XrefRangeEnd = 791124, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetPropertyBlock(MaterialPropertyBlock block)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)block);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPropertyBlock_Public_Void_MaterialPropertyBlock_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791147, RefRangeEnd = 791149, XrefRangeStart = 791125, XrefRangeEnd = 791147, MetadataInitTokenRva = 46273984L, MetadataInitFlagRva = 59848136L)]
	public unsafe static SkeletonPartsRenderer NewPartsRendererGameObject(Transform parent, string name, int sortingOrder = 0)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &sortingOrder;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewPartsRendererGameObject_Public_Static_SkeletonPartsRenderer_Transform_String_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonPartsRenderer>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791149, XrefRangeEnd = 791155, MetadataInitTokenRva = 46274048L, MetadataInitFlagRva = 59848137L)]
	public unsafe SkeletonPartsRenderer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonPartsRenderer>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonPartsRenderer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
