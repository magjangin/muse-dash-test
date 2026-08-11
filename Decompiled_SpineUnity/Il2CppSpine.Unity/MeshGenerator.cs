using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity;

[System.Serializable]
public class MeshGenerator : Il2CppSystem.Object
{
	[System.Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct Settings
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_useClipping;

		private static readonly System.IntPtr NativeFieldInfoPtr_zSpacing;

		private static readonly System.IntPtr NativeFieldInfoPtr_pmaVertexColors;

		private static readonly System.IntPtr NativeFieldInfoPtr_tintBlack;

		private static readonly System.IntPtr NativeFieldInfoPtr_canvasGroupTintBlack;

		private static readonly System.IntPtr NativeFieldInfoPtr_calculateTangents;

		private static readonly System.IntPtr NativeFieldInfoPtr_addNormals;

		private static readonly System.IntPtr NativeFieldInfoPtr_immutableTriangles;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Default_Public_Static_get_Settings_0;

		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.U1)]
		public bool useClipping;

		[FieldOffset(4)]
		public float zSpacing;

		[FieldOffset(8)]
		[MarshalAs(UnmanagedType.U1)]
		public bool pmaVertexColors;

		[FieldOffset(9)]
		[MarshalAs(UnmanagedType.U1)]
		public bool tintBlack;

		[FieldOffset(10)]
		[MarshalAs(UnmanagedType.U1)]
		public bool canvasGroupTintBlack;

		[FieldOffset(11)]
		[MarshalAs(UnmanagedType.U1)]
		public bool calculateTangents;

		[FieldOffset(12)]
		[MarshalAs(UnmanagedType.U1)]
		public bool addNormals;

		[FieldOffset(13)]
		[MarshalAs(UnmanagedType.U1)]
		public bool immutableTriangles;

		public unsafe static Settings Default
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Default_Public_Static_get_Settings_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(Settings*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static Settings()
		{
			Il2CppClassPointerStore<Settings>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "Settings");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Settings>.NativeClassPtr);
			NativeFieldInfoPtr_useClipping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "useClipping");
			NativeFieldInfoPtr_zSpacing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "zSpacing");
			NativeFieldInfoPtr_pmaVertexColors = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "pmaVertexColors");
			NativeFieldInfoPtr_tintBlack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "tintBlack");
			NativeFieldInfoPtr_canvasGroupTintBlack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "canvasGroupTintBlack");
			NativeFieldInfoPtr_calculateTangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "calculateTangents");
			NativeFieldInfoPtr_addNormals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "addNormals");
			NativeFieldInfoPtr_immutableTriangles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Settings>.NativeClassPtr, "immutableTriangles");
			NativeMethodInfoPtr_get_Default_Public_Static_get_Settings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Settings>.NativeClassPtr, 100664363);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Settings>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_settings;

	private static readonly System.IntPtr NativeFieldInfoPtr_BoundsMinDefault;

	private static readonly System.IntPtr NativeFieldInfoPtr_BoundsMaxDefault;

	private static readonly System.IntPtr NativeFieldInfoPtr_vertexBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_uvBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_colorBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_submeshes;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshBoundsMin;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshBoundsMax;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshBoundsThickness;

	private static readonly System.IntPtr NativeFieldInfoPtr_submeshIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_clipper;

	private static readonly System.IntPtr NativeFieldInfoPtr_tempVerts;

	private static readonly System.IntPtr NativeFieldInfoPtr_regionTriangles;

	private static readonly System.IntPtr NativeFieldInfoPtr_normals;

	private static readonly System.IntPtr NativeFieldInfoPtr_tangents;

	private static readonly System.IntPtr NativeFieldInfoPtr_tempTanBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_uv2;

	private static readonly System.IntPtr NativeFieldInfoPtr_uv3;

	private static readonly System.IntPtr NativeFieldInfoPtr_AttachmentVerts;

	private static readonly System.IntPtr NativeFieldInfoPtr_AttachmentUVs;

	private static readonly System.IntPtr NativeFieldInfoPtr_AttachmentColors32;

	private static readonly System.IntPtr NativeFieldInfoPtr_AttachmentIndices;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Buffers_Public_get_MeshGeneratorBuffers_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GenerateSingleSubmeshInstruction_Public_Static_Void_SkeletonRendererInstruction_Skeleton_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RequiresMultipleSubmeshesByDrawOrder_Public_Static_Boolean_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GenerateSkeletonRendererInstruction_Public_Static_Void_SkeletonRendererInstruction_Skeleton_Dictionary_2_Slot_Material_List_1_Slot_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryReplaceMaterials_Public_Static_Void_ExposedList_1_SubmeshInstruction_Dictionary_2_Material_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Begin_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddSubmesh_Public_Void_SubmeshInstruction_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BuildMesh_Public_Void_SkeletonRendererInstruction_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BuildMeshWithArrays_Public_Void_SkeletonRendererInstruction_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ScaleVertexData_Public_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddAttachmentTintBlack_Private_Void_Single_Single_Single_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FillVertexData_Public_Void_Mesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FillLateVertexData_Public_Void_Mesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FillTriangles_Public_Void_Mesh_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnsureVertexCapacity_Public_Void_Int32_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SolveTangents2DEnsureSize_Internal_Static_Void_byref_Il2CppStructArray_1_Vector4_byref_Il2CppStructArray_1_Vector2_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SolveTangents2DTriangles_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Int32_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector2_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SolveTangents2DBuffer_Internal_Static_Void_Il2CppStructArray_1_Vector4_Il2CppStructArray_1_Vector2_Int32_0;

	public unsafe Settings settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_settings);
			return *(Settings*)num;
		}
		set
		{
			*(Settings*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_settings)) = settings;
		}
	}

	public unsafe static float BoundsMinDefault
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BoundsMinDefault, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BoundsMinDefault, (void*)(&num));
		}
	}

	public unsafe static float BoundsMaxDefault
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BoundsMaxDefault, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BoundsMaxDefault, (void*)(&num));
		}
	}

	public unsafe ExposedList<Vector3> vertexBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertexBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Vector3>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertexBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Vector2> uvBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uvBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Vector2>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uvBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Color32> colorBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_colorBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Color32>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_colorBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<ExposedList<int>> submeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_submeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<ExposedList<int>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_submeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe Vector2 meshBoundsMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshBoundsMin);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshBoundsMin)) = vector;
		}
	}

	public unsafe Vector2 meshBoundsMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshBoundsMax);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshBoundsMax)) = vector;
		}
	}

	public unsafe float meshBoundsThickness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshBoundsThickness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_meshBoundsThickness)) = num;
		}
	}

	public unsafe int submeshIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_submeshIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_submeshIndex)) = num;
		}
	}

	public unsafe SkeletonClipping clipper
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipper);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonClipping>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipper)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonClipping));
		}
	}

	public unsafe Il2CppStructArray<float> tempVerts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tempVerts);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tempVerts)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<int> regionTriangles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_regionTriangles);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_regionTriangles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Vector3> normals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_normals);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_normals)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Vector4> tangents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tangents);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector4>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tangents)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Vector2> tempTanBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tempTanBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tempTanBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe ExposedList<Vector2> uv2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uv2);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Vector2>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uv2)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Vector2> uv3
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uv3);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Vector2>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uv3)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe static List<Vector3> AttachmentVerts
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttachmentVerts, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Vector3>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttachmentVerts, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe static List<Vector2> AttachmentUVs
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttachmentUVs, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Vector2>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttachmentUVs, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe static List<Color32> AttachmentColors32
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttachmentColors32, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Color32>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttachmentColors32, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe static List<int> AttachmentIndices
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AttachmentIndices, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<int>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AttachmentIndices, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe int VertexCount
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 731814, RefRangeEnd = 731817, XrefRangeStart = 731814, XrefRangeEnd = 731817, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe MeshGeneratorBuffers Buffers
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 792653, RefRangeEnd = 792655, XrefRangeStart = 792649, XrefRangeEnd = 792653, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Buffers_Public_get_MeshGeneratorBuffers_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new MeshGeneratorBuffers(pointer);
		}
	}

	static MeshGenerator()
	{
		Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "MeshGenerator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr);
		NativeFieldInfoPtr_settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "settings");
		NativeFieldInfoPtr_BoundsMinDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "BoundsMinDefault");
		NativeFieldInfoPtr_BoundsMaxDefault = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "BoundsMaxDefault");
		NativeFieldInfoPtr_vertexBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "vertexBuffer");
		NativeFieldInfoPtr_uvBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "uvBuffer");
		NativeFieldInfoPtr_colorBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "colorBuffer");
		NativeFieldInfoPtr_submeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "submeshes");
		NativeFieldInfoPtr_meshBoundsMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "meshBoundsMin");
		NativeFieldInfoPtr_meshBoundsMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "meshBoundsMax");
		NativeFieldInfoPtr_meshBoundsThickness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "meshBoundsThickness");
		NativeFieldInfoPtr_submeshIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "submeshIndex");
		NativeFieldInfoPtr_clipper = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "clipper");
		NativeFieldInfoPtr_tempVerts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "tempVerts");
		NativeFieldInfoPtr_regionTriangles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "regionTriangles");
		NativeFieldInfoPtr_normals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "normals");
		NativeFieldInfoPtr_tangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "tangents");
		NativeFieldInfoPtr_tempTanBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "tempTanBuffer");
		NativeFieldInfoPtr_uv2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "uv2");
		NativeFieldInfoPtr_uv3 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "uv3");
		NativeFieldInfoPtr_AttachmentVerts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "AttachmentVerts");
		NativeFieldInfoPtr_AttachmentUVs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "AttachmentUVs");
		NativeFieldInfoPtr_AttachmentColors32 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "AttachmentColors32");
		NativeFieldInfoPtr_AttachmentIndices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, "AttachmentIndices");
		NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664342);
		NativeMethodInfoPtr_get_Buffers_Public_get_MeshGeneratorBuffers_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664343);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664344);
		NativeMethodInfoPtr_GenerateSingleSubmeshInstruction_Public_Static_Void_SkeletonRendererInstruction_Skeleton_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664345);
		NativeMethodInfoPtr_RequiresMultipleSubmeshesByDrawOrder_Public_Static_Boolean_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664346);
		NativeMethodInfoPtr_GenerateSkeletonRendererInstruction_Public_Static_Void_SkeletonRendererInstruction_Skeleton_Dictionary_2_Slot_Material_List_1_Slot_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664347);
		NativeMethodInfoPtr_TryReplaceMaterials_Public_Static_Void_ExposedList_1_SubmeshInstruction_Dictionary_2_Material_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664348);
		NativeMethodInfoPtr_Begin_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664349);
		NativeMethodInfoPtr_AddSubmesh_Public_Void_SubmeshInstruction_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664350);
		NativeMethodInfoPtr_BuildMesh_Public_Void_SkeletonRendererInstruction_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664351);
		NativeMethodInfoPtr_BuildMeshWithArrays_Public_Void_SkeletonRendererInstruction_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664352);
		NativeMethodInfoPtr_ScaleVertexData_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664353);
		NativeMethodInfoPtr_AddAttachmentTintBlack_Private_Void_Single_Single_Single_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664354);
		NativeMethodInfoPtr_FillVertexData_Public_Void_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664355);
		NativeMethodInfoPtr_FillLateVertexData_Public_Void_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664356);
		NativeMethodInfoPtr_FillTriangles_Public_Void_Mesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664357);
		NativeMethodInfoPtr_EnsureVertexCapacity_Public_Void_Int32_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664358);
		NativeMethodInfoPtr_SolveTangents2DEnsureSize_Internal_Static_Void_byref_Il2CppStructArray_1_Vector4_byref_Il2CppStructArray_1_Vector2_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664359);
		NativeMethodInfoPtr_SolveTangents2DTriangles_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Int32_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664360);
		NativeMethodInfoPtr_SolveTangents2DBuffer_Internal_Static_Void_Il2CppStructArray_1_Vector4_Il2CppStructArray_1_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr, 100664361);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 792698, RefRangeEnd = 792702, XrefRangeStart = 792655, XrefRangeEnd = 792698, MetadataInitTokenRva = 46352648L, MetadataInitFlagRva = 59827172L)]
	public unsafe MeshGenerator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshGenerator>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792729, RefRangeEnd = 792731, XrefRangeStart = 792702, XrefRangeEnd = 792729, MetadataInitTokenRva = 46352332L, MetadataInitFlagRva = 59827173L)]
	public unsafe static void GenerateSingleSubmeshInstruction(SkeletonRendererInstruction instructionOutput, Skeleton skeleton, Material material)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instructionOutput);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateSingleSubmeshInstruction_Public_Static_Void_SkeletonRendererInstruction_Skeleton_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 792748, RefRangeEnd = 792749, XrefRangeStart = 792731, XrefRangeEnd = 792748, MetadataInitTokenRva = 46352388L, MetadataInitFlagRva = 59827174L)]
	public unsafe static bool RequiresMultipleSubmeshesByDrawOrder(Skeleton skeleton)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RequiresMultipleSubmeshesByDrawOrder_Public_Static_Boolean_Skeleton_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792799, RefRangeEnd = 792801, XrefRangeStart = 792749, XrefRangeEnd = 792799, MetadataInitTokenRva = 46352364L, MetadataInitFlagRva = 59827175L)]
	public unsafe static void GenerateSkeletonRendererInstruction(SkeletonRendererInstruction instructionOutput, Skeleton skeleton, Dictionary<Slot, Material> customSlotMaterials, List<Slot> separatorSlots, bool generateMeshOverride, bool immutableTriangles = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instructionOutput);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)customSlotMaterials);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)separatorSlots);
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &generateMeshOverride;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &immutableTriangles;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateSkeletonRendererInstruction_Public_Static_Void_SkeletonRendererInstruction_Skeleton_Dictionary_2_Slot_Material_List_1_Slot_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792806, RefRangeEnd = 792808, XrefRangeStart = 792801, XrefRangeEnd = 792806, MetadataInitTokenRva = 46352560L, MetadataInitFlagRva = 59827176L)]
	public unsafe static void TryReplaceMaterials(ExposedList<SubmeshInstruction> workingSubmeshInstructions, Dictionary<Material, Material> customMaterialOverride)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)workingSubmeshInstructions);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)customMaterialOverride);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryReplaceMaterials_Public_Static_Void_ExposedList_1_SubmeshInstruction_Dictionary_2_Material_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 792816, RefRangeEnd = 792822, XrefRangeStart = 792808, XrefRangeEnd = 792816, MetadataInitTokenRva = 46352124L, MetadataInitFlagRva = 59827177L)]
	public unsafe void Begin()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Begin_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 792875, RefRangeEnd = 792880, XrefRangeStart = 792822, XrefRangeEnd = 792875, MetadataInitTokenRva = 46352096L, MetadataInitFlagRva = 59827178L)]
	public unsafe void AddSubmesh(SubmeshInstruction instruction, bool updateTriangles = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)instruction));
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &updateTriangles;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddSubmesh_Public_Void_SubmeshInstruction_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 792881, RefRangeEnd = 792882, XrefRangeStart = 792880, XrefRangeEnd = 792881, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BuildMesh(SkeletonRendererInstruction instruction, bool updateTriangles)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instruction);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &updateTriangles;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BuildMesh_Public_Void_SkeletonRendererInstruction_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 792955, RefRangeEnd = 792958, XrefRangeStart = 792882, XrefRangeEnd = 792955, MetadataInitTokenRva = 46352168L, MetadataInitFlagRva = 59827179L)]
	public unsafe void BuildMeshWithArrays(SkeletonRendererInstruction instruction, bool updateTriangles)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instruction);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &updateTriangles;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BuildMeshWithArrays_Public_Void_SkeletonRendererInstruction_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792966, RefRangeEnd = 792968, XrefRangeStart = 792958, XrefRangeEnd = 792966, MetadataInitTokenRva = 46352428L, MetadataInitFlagRva = 59827180L)]
	public unsafe void ScaleVertexData(float scale)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&scale);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScaleVertexData_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 792986, RefRangeEnd = 792987, XrefRangeStart = 792968, XrefRangeEnd = 792986, MetadataInitTokenRva = 46352032L, MetadataInitFlagRva = 59827181L)]
	public unsafe void AddAttachmentTintBlack(float r2, float g2, float b2, float a, int vertexCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&r2);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &g2;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &b2;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &a;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &vertexCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddAttachmentTintBlack_Private_Void_Single_Single_Single_Single_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 793018, RefRangeEnd = 793022, XrefRangeStart = 792987, XrefRangeEnd = 793018, MetadataInitTokenRva = 46352276L, MetadataInitFlagRva = 59827182L)]
	public unsafe void FillVertexData(Mesh mesh)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FillVertexData_Public_Void_Mesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 793042, RefRangeEnd = 793046, XrefRangeStart = 793022, XrefRangeEnd = 793042, MetadataInitTokenRva = 46352224L, MetadataInitFlagRva = 59827183L)]
	public unsafe void FillLateVertexData(Mesh mesh)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FillLateVertexData_Public_Void_Mesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 793048, RefRangeEnd = 793052, XrefRangeStart = 793046, XrefRangeEnd = 793048, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FillTriangles(Mesh mesh)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FillTriangles_Public_Void_Mesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793082, RefRangeEnd = 793083, XrefRangeStart = 793052, XrefRangeEnd = 793082, MetadataInitTokenRva = 46352216L, MetadataInitFlagRva = 59827184L)]
	public unsafe void EnsureVertexCapacity(int minimumVertexCount, bool inlcudeTintBlack = false, bool includeTangents = false, bool includeNormals = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&minimumVertexCount);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &inlcudeTintBlack;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeTangents;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeNormals;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureVertexCapacity_Public_Void_Int32_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793083, XrefRangeEnd = 793090, MetadataInitTokenRva = 46352500L, MetadataInitFlagRva = 59827185L)]
	public unsafe static void SolveTangents2DEnsureSize(ref Il2CppStructArray<Vector4> tangentBuffer, ref Il2CppStructArray<Vector2> tempTanBuffer, int vertexCount, int vertexBufferLength)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tangentBuffer);
		*ptr = (nint)(&intPtr);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		System.IntPtr intPtr2 = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tempTanBuffer);
		*(System.IntPtr**)num = &intPtr2;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &vertexCount;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &vertexBufferLength;
		Unsafe.SkipInit(out System.IntPtr intPtr4);
		System.IntPtr intPtr3 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SolveTangents2DEnsureSize_Internal_Static_Void_byref_Il2CppStructArray_1_Vector4_byref_Il2CppStructArray_1_Vector2_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr4);
		Il2CppException.RaiseExceptionIfNecessary(intPtr4);
		System.IntPtr intPtr5 = intPtr;
		tangentBuffer = ((intPtr5 == (System.IntPtr)0) ? null : new Il2CppStructArray<Vector4>(intPtr5));
		System.IntPtr intPtr6 = intPtr2;
		tempTanBuffer = ((intPtr6 == (System.IntPtr)0) ? null : new Il2CppStructArray<Vector2>(intPtr6));
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793091, RefRangeEnd = 793092, XrefRangeStart = 793090, XrefRangeEnd = 793091, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SolveTangents2DTriangles(Il2CppStructArray<Vector2> tempTanBuffer, Il2CppStructArray<int> triangles, int triangleCount, Il2CppStructArray<Vector3> vertices, Il2CppStructArray<Vector2> uvs, int vertexCount)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tempTanBuffer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)triangles);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &triangleCount;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uvs);
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &vertexCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SolveTangents2DTriangles_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Int32_Int32_Il2CppStructArray_1_Vector3_Il2CppStructArray_1_Vector2_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793097, RefRangeEnd = 793098, XrefRangeStart = 793092, XrefRangeEnd = 793097, MetadataInitTokenRva = 46352488L, MetadataInitFlagRva = 59827186L)]
	public unsafe static void SolveTangents2DBuffer(Il2CppStructArray<Vector4> tangents, Il2CppStructArray<Vector2> tempTanBuffer, int vertexCount)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tangents);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tempTanBuffer);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &vertexCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SolveTangents2DBuffer_Internal_Static_Void_Il2CppStructArray_1_Vector4_Il2CppStructArray_1_Vector2_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MeshGenerator(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
