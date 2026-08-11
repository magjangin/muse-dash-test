using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class MeshRendererBuffers : Il2CppSystem.Object
{
	public class SmartMesh : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_mesh;

		private static readonly System.IntPtr NativeFieldInfoPtr_instructionUsed;

		private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Mesh mesh
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mesh);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh));
			}
		}

		public unsafe SkeletonRendererInstruction instructionUsed
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_instructionUsed);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRendererInstruction>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_instructionUsed)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRendererInstruction));
			}
		}

		static SmartMesh()
		{
			Il2CppClassPointerStore<SmartMesh>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, "SmartMesh");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SmartMesh>.NativeClassPtr);
			NativeFieldInfoPtr_mesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SmartMesh>.NativeClassPtr, "mesh");
			NativeFieldInfoPtr_instructionUsed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SmartMesh>.NativeClassPtr, "instructionUsed");
			NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SmartMesh>.NativeClassPtr, 100664372);
			NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SmartMesh>.NativeClassPtr, 100664373);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SmartMesh>.NativeClassPtr, 100664374);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793098, XrefRangeEnd = 793101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Clear()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 793109, RefRangeEnd = 793111, XrefRangeStart = 793101, XrefRangeEnd = 793109, MetadataInitTokenRva = 46286204L, MetadataInitFlagRva = 59827196L)]
		public unsafe virtual void Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793111, XrefRangeEnd = 793119, MetadataInitTokenRva = 46286264L, MetadataInitFlagRva = 59827197L)]
		public unsafe SmartMesh()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SmartMesh>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SmartMesh(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_doubleBufferedMesh;

	private static readonly System.IntPtr NativeFieldInfoPtr_submeshMaterials;

	private static readonly System.IntPtr NativeFieldInfoPtr_sharedMaterials;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetUpdatedSharedMaterialsArray_Public_Il2CppReferenceArray_1_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MaterialsChangedInLastUpdate_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateSharedMaterials_Public_Void_ExposedList_1_SubmeshInstruction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNextMesh_Public_SmartMesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe DoubleBuffered<SmartMesh> doubleBufferedMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_doubleBufferedMesh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DoubleBuffered<SmartMesh>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_doubleBufferedMesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)doubleBuffered));
		}
	}

	public unsafe ExposedList<Material> submeshMaterials
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_submeshMaterials);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Material>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_submeshMaterials)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe Il2CppReferenceArray<Material> sharedMaterials
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sharedMaterials);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sharedMaterials)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static MeshRendererBuffers()
	{
		Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "MeshRendererBuffers");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr);
		NativeFieldInfoPtr_doubleBufferedMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, "doubleBufferedMesh");
		NativeFieldInfoPtr_submeshMaterials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, "submeshMaterials");
		NativeFieldInfoPtr_sharedMaterials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, "sharedMaterials");
		NativeMethodInfoPtr_Initialize_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, 100664364);
		NativeMethodInfoPtr_GetUpdatedSharedMaterialsArray_Public_Il2CppReferenceArray_1_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, 100664365);
		NativeMethodInfoPtr_MaterialsChangedInLastUpdate_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, 100664366);
		NativeMethodInfoPtr_UpdateSharedMaterials_Public_Void_ExposedList_1_SubmeshInstruction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, 100664367);
		NativeMethodInfoPtr_GetNextMesh_Public_SmartMesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, 100664368);
		NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, 100664369);
		NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, 100664370);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr, 100664371);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 793136, RefRangeEnd = 793138, XrefRangeStart = 793119, XrefRangeEnd = 793136, MetadataInitTokenRva = 46352948L, MetadataInitFlagRva = 59827189L)]
	public unsafe void Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 793142, RefRangeEnd = 793144, XrefRangeStart = 793138, XrefRangeEnd = 793142, MetadataInitTokenRva = 46352912L, MetadataInitFlagRva = 59827190L)]
	public unsafe Il2CppReferenceArray<Material> GetUpdatedSharedMaterialsArray()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetUpdatedSharedMaterialsArray_Public_Il2CppReferenceArray_1_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 793144, RefRangeEnd = 793146, XrefRangeStart = 793144, XrefRangeEnd = 793144, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool MaterialsChangedInLastUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MaterialsChangedInLastUpdate_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 793151, RefRangeEnd = 793153, XrefRangeStart = 793146, XrefRangeEnd = 793151, MetadataInitTokenRva = 46352996L, MetadataInitFlagRva = 59827191L)]
	public unsafe void UpdateSharedMaterials(ExposedList<SubmeshInstruction> instructions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instructions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSharedMaterials_Public_Void_ExposedList_1_SubmeshInstruction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 793157, RefRangeEnd = 793159, XrefRangeStart = 793153, XrefRangeEnd = 793157, MetadataInitTokenRva = 46352876L, MetadataInitFlagRva = 59827192L)]
	public unsafe SmartMesh GetNextMesh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNextMesh_Public_SmartMesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SmartMesh>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793166, RefRangeEnd = 793167, XrefRangeStart = 793159, XrefRangeEnd = 793166, MetadataInitTokenRva = 46352796L, MetadataInitFlagRva = 59827193L)]
	public unsafe void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793175, RefRangeEnd = 793176, XrefRangeStart = 793167, XrefRangeEnd = 793175, MetadataInitTokenRva = 46352840L, MetadataInitFlagRva = 59827194L)]
	public unsafe virtual void Dispose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 793186, RefRangeEnd = 793189, XrefRangeStart = 793176, XrefRangeEnd = 793186, MetadataInitTokenRva = 46353020L, MetadataInitFlagRva = 59827195L)]
	public unsafe MeshRendererBuffers()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshRendererBuffers>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MeshRendererBuffers(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
