using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class IkConstraintData : ConstraintData
{
	private static readonly IntPtr NativeFieldInfoPtr_bones;

	private static readonly IntPtr NativeFieldInfoPtr_target;

	private static readonly IntPtr NativeFieldInfoPtr_bendDirection;

	private static readonly IntPtr NativeFieldInfoPtr_compress;

	private static readonly IntPtr NativeFieldInfoPtr_stretch;

	private static readonly IntPtr NativeFieldInfoPtr_uniform;

	private static readonly IntPtr NativeFieldInfoPtr_mix;

	private static readonly IntPtr NativeFieldInfoPtr_softness;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	public unsafe ExposedList<BoneData> bones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<BoneData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe BoneData target
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<BoneData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)boneData));
		}
	}

	public unsafe int bendDirection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bendDirection);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bendDirection)) = num;
		}
	}

	public unsafe bool compress
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_compress);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_compress)) = flag;
		}
	}

	public unsafe bool stretch
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stretch);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stretch)) = flag;
		}
	}

	public unsafe bool uniform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uniform);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uniform)) = flag;
		}
	}

	public unsafe float mix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mix)) = num;
		}
	}

	public unsafe float softness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_softness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_softness)) = num;
		}
	}

	static IkConstraintData()
	{
		Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "IkConstraintData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr);
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, "target");
		NativeFieldInfoPtr_bendDirection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, "bendDirection");
		NativeFieldInfoPtr_compress = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, "compress");
		NativeFieldInfoPtr_stretch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, "stretch");
		NativeFieldInfoPtr_uniform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, "uniform");
		NativeFieldInfoPtr_mix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, "mix");
		NativeFieldInfoPtr_softness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, "softness");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr, 100663687);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784208, RefRangeEnd = 784210, XrefRangeStart = 784192, XrefRangeEnd = 784208, MetadataInitTokenRva = 46293052L, MetadataInitFlagRva = 59849681L)]
	public unsafe IkConstraintData(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<IkConstraintData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public IkConstraintData(IntPtr pointer)
		: base(pointer)
	{
	}
}
