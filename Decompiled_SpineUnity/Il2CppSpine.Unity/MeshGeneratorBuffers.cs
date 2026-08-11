using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppSpine.Unity;

public sealed class MeshGeneratorBuffers : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_vertexCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_vertexBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_uvBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_colorBuffer;

	private static readonly System.IntPtr NativeFieldInfoPtr_meshGenerator;

	public unsafe int vertexCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertexCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertexCount)) = num;
		}
	}

	public unsafe Il2CppStructArray<Vector3> vertexBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertexBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertexBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Vector2> uvBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uvBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uvBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<Color32> colorBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_colorBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Color32>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_colorBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
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

	static MeshGeneratorBuffers()
	{
		Il2CppClassPointerStore<MeshGeneratorBuffers>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "MeshGeneratorBuffers");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MeshGeneratorBuffers>.NativeClassPtr);
		NativeFieldInfoPtr_vertexCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGeneratorBuffers>.NativeClassPtr, "vertexCount");
		NativeFieldInfoPtr_vertexBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGeneratorBuffers>.NativeClassPtr, "vertexBuffer");
		NativeFieldInfoPtr_uvBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGeneratorBuffers>.NativeClassPtr, "uvBuffer");
		NativeFieldInfoPtr_colorBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGeneratorBuffers>.NativeClassPtr, "colorBuffer");
		NativeFieldInfoPtr_meshGenerator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MeshGeneratorBuffers>.NativeClassPtr, "meshGenerator");
	}

	public MeshGeneratorBuffers(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public MeshGeneratorBuffers()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshGeneratorBuffers>.NativeClassPtr))
	{
	}
}
