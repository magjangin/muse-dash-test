using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class TransformConstraint : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_data;

	private static readonly System.IntPtr NativeFieldInfoPtr_bones;

	private static readonly System.IntPtr NativeFieldInfoPtr_target;

	private static readonly System.IntPtr NativeFieldInfoPtr_rotateMix;

	private static readonly System.IntPtr NativeFieldInfoPtr_translateMix;

	private static readonly System.IntPtr NativeFieldInfoPtr_scaleMix;

	private static readonly System.IntPtr NativeFieldInfoPtr_shearMix;

	private static readonly System.IntPtr NativeFieldInfoPtr_active;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_TransformConstraintData_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Virtual_Final_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyAbsoluteWorld_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyRelativeWorld_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyAbsoluteLocal_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyRelativeLocal_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	public unsafe TransformConstraintData data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TransformConstraintData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transformConstraintData));
		}
	}

	public unsafe ExposedList<Bone> bones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Bone>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe Bone target
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Bone>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone));
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

	public unsafe bool active
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_active);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_active)) = flag;
		}
	}

	static TransformConstraint()
	{
		Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "TransformConstraint");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr);
		NativeFieldInfoPtr_data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, "data");
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, "target");
		NativeFieldInfoPtr_rotateMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, "rotateMix");
		NativeFieldInfoPtr_translateMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, "translateMix");
		NativeFieldInfoPtr_scaleMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, "scaleMix");
		NativeFieldInfoPtr_shearMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, "shearMix");
		NativeFieldInfoPtr_active = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, "active");
		NativeMethodInfoPtr__ctor_Public_Void_TransformConstraintData_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, 100663832);
		NativeMethodInfoPtr_Update_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, 100663833);
		NativeMethodInfoPtr_ApplyAbsoluteWorld_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, 100663834);
		NativeMethodInfoPtr_ApplyRelativeWorld_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, 100663835);
		NativeMethodInfoPtr_ApplyAbsoluteLocal_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, 100663836);
		NativeMethodInfoPtr_ApplyRelativeLocal_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, 100663837);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr, 100663838);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 787948, RefRangeEnd = 787949, XrefRangeStart = 787927, XrefRangeEnd = 787948, MetadataInitTokenRva = 47210804L, MetadataInitFlagRva = 59867982L)]
	public unsafe TransformConstraint(TransformConstraintData data, Skeleton skeleton)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TransformConstraint>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_TransformConstraintData_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 787949, XrefRangeEnd = 787954, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 787998, RefRangeEnd = 787999, XrefRangeStart = 787954, XrefRangeEnd = 787998, MetadataInitTokenRva = 47210716L, MetadataInitFlagRva = 59867983L)]
	public unsafe void ApplyAbsoluteWorld()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyAbsoluteWorld_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788031, RefRangeEnd = 788032, XrefRangeStart = 787999, XrefRangeEnd = 788031, MetadataInitTokenRva = 47210780L, MetadataInitFlagRva = 59867984L)]
	public unsafe void ApplyRelativeWorld()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyRelativeWorld_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788037, RefRangeEnd = 788038, XrefRangeStart = 788032, XrefRangeEnd = 788037, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyAbsoluteLocal()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyAbsoluteLocal_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788043, RefRangeEnd = 788044, XrefRangeStart = 788038, XrefRangeEnd = 788043, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyRelativeLocal()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyRelativeLocal_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 64785, RefRangeEnd = 64788, XrefRangeStart = 64785, XrefRangeEnd = 64788, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public TransformConstraint(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
