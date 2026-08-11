using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class PointAttachment : Attachment
{
	private static readonly IntPtr NativeFieldInfoPtr_x;

	private static readonly IntPtr NativeFieldInfoPtr_y;

	private static readonly IntPtr NativeFieldInfoPtr_rotation;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_ComputeWorldPosition_Public_Void_Bone_byref_Single_byref_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ComputeWorldRotation_Public_Single_Bone_0;

	private static readonly IntPtr NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0;

	public unsafe float x
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_x);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_x)) = num;
		}
	}

	public unsafe float y
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_y);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_y)) = num;
		}
	}

	public unsafe float rotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotation)) = num;
		}
	}

	static PointAttachment()
	{
		Il2CppClassPointerStore<PointAttachment>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "PointAttachment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr);
		NativeFieldInfoPtr_x = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr, "x");
		NativeFieldInfoPtr_y = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr, "y");
		NativeFieldInfoPtr_rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr, "rotation");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr, 100663576);
		NativeMethodInfoPtr_ComputeWorldPosition_Public_Void_Bone_byref_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr, 100663577);
		NativeMethodInfoPtr_ComputeWorldRotation_Public_Single_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr, 100663578);
		NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr, 100663579);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 783396, RefRangeEnd = 783398, XrefRangeStart = 783396, XrefRangeEnd = 783398, MetadataInitTokenRva = 47239392L, MetadataInitFlagRva = 59849646L)]
	public unsafe PointAttachment(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PointAttachment>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783506, RefRangeEnd = 783507, XrefRangeStart = 783506, XrefRangeEnd = 783506, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ComputeWorldPosition(Bone bone, out float ox, out float oy)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref ox);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref oy);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeWorldPosition_Public_Void_Bone_byref_Single_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783520, RefRangeEnd = 783521, XrefRangeStart = 783507, XrefRangeEnd = 783520, MetadataInitTokenRva = 46294688L, MetadataInitFlagRva = 59849705L)]
	public unsafe float ComputeWorldRotation(Bone bone)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeWorldRotation_Public_Single_Bone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783521, XrefRangeEnd = 783527, MetadataInitTokenRva = 46294728L, MetadataInitFlagRva = 59849706L)]
	public unsafe override Attachment Copy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	public PointAttachment(IntPtr pointer)
		: base(pointer)
	{
	}
}
