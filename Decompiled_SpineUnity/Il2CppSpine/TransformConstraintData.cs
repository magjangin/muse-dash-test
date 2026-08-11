using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class TransformConstraintData : ConstraintData
{
	private static readonly IntPtr NativeFieldInfoPtr_bones;

	private static readonly IntPtr NativeFieldInfoPtr_target;

	private static readonly IntPtr NativeFieldInfoPtr_rotateMix;

	private static readonly IntPtr NativeFieldInfoPtr_translateMix;

	private static readonly IntPtr NativeFieldInfoPtr_scaleMix;

	private static readonly IntPtr NativeFieldInfoPtr_shearMix;

	private static readonly IntPtr NativeFieldInfoPtr_offsetRotation;

	private static readonly IntPtr NativeFieldInfoPtr_offsetX;

	private static readonly IntPtr NativeFieldInfoPtr_offsetY;

	private static readonly IntPtr NativeFieldInfoPtr_offsetScaleX;

	private static readonly IntPtr NativeFieldInfoPtr_offsetScaleY;

	private static readonly IntPtr NativeFieldInfoPtr_offsetShearY;

	private static readonly IntPtr NativeFieldInfoPtr_relative;

	private static readonly IntPtr NativeFieldInfoPtr_local;

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

	public unsafe float rotateMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotateMix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotateMix)) = num;
		}
	}

	public unsafe float translateMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_translateMix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_translateMix)) = num;
		}
	}

	public unsafe float scaleMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleMix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleMix)) = num;
		}
	}

	public unsafe float shearMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shearMix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shearMix)) = num;
		}
	}

	public unsafe float offsetRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetRotation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetRotation)) = num;
		}
	}

	public unsafe float offsetX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetX)) = num;
		}
	}

	public unsafe float offsetY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetY)) = num;
		}
	}

	public unsafe float offsetScaleX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetScaleX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetScaleX)) = num;
		}
	}

	public unsafe float offsetScaleY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetScaleY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetScaleY)) = num;
		}
	}

	public unsafe float offsetShearY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetShearY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetShearY)) = num;
		}
	}

	public unsafe bool relative
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_relative);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_relative)) = flag;
		}
	}

	public unsafe bool local
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_local);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_local)) = flag;
		}
	}

	static TransformConstraintData()
	{
		Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "TransformConstraintData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr);
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "target");
		NativeFieldInfoPtr_rotateMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "rotateMix");
		NativeFieldInfoPtr_translateMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "translateMix");
		NativeFieldInfoPtr_scaleMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "scaleMix");
		NativeFieldInfoPtr_shearMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "shearMix");
		NativeFieldInfoPtr_offsetRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "offsetRotation");
		NativeFieldInfoPtr_offsetX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "offsetX");
		NativeFieldInfoPtr_offsetY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "offsetY");
		NativeFieldInfoPtr_offsetScaleX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "offsetScaleX");
		NativeFieldInfoPtr_offsetScaleY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "offsetScaleY");
		NativeFieldInfoPtr_offsetShearY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "offsetShearY");
		NativeFieldInfoPtr_relative = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "relative");
		NativeFieldInfoPtr_local = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, "local");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr, 100663839);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788051, RefRangeEnd = 788052, XrefRangeStart = 788044, XrefRangeEnd = 788051, MetadataInitTokenRva = 47210668L, MetadataInitFlagRva = 59867985L)]
	public unsafe TransformConstraintData(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TransformConstraintData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TransformConstraintData(IntPtr pointer)
		: base(pointer)
	{
	}
}
