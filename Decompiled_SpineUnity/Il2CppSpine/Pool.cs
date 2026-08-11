using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppSpine;

public class Pool<T> : Il2CppSystem.Object where T : class, new()
{
	public class IPoolable : Il2CppObjectBase
	{
		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Abstract_Virtual_New_Void_0;

		static IPoolable()
		{
			Il2CppClassPointerStore<IPoolable>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, "IPoolable"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
			NativeMethodInfoPtr_Reset_Public_Abstract_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IPoolable>.NativeClassPtr, 100663484);
		}

		[CallerCount(0)]
		public unsafe virtual void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Abstract_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public IPoolable(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_max;

	private static readonly System.IntPtr NativeFieldInfoPtr_freeObjects;

	private static readonly System.IntPtr NativeFieldInfoPtr__Peak_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Peak_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Peak_Private_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Obtain_Public_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Free_Public_Void_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Protected_Void_T_0;

	public unsafe int max
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_max);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_max)) = num;
		}
	}

	public unsafe Stack<T> freeObjects
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_freeObjects);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stack<T>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_freeObjects)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stack));
		}
	}

	public unsafe int _Peak_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Peak_k__BackingField);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Peak_k__BackingField)) = num;
		}
	}

	public unsafe int Peak
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 51107, RefRangeEnd = 51109, XrefRangeStart = 51107, XrefRangeEnd = 51109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Peak_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Peak_Private_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Pool()
	{
		Il2CppClassPointerStore<Pool<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "Pool`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr);
		NativeFieldInfoPtr_max = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, "max");
		NativeFieldInfoPtr_freeObjects = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, "freeObjects");
		NativeFieldInfoPtr__Peak_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, "<Peak>k__BackingField");
		NativeMethodInfoPtr_get_Peak_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, 100663478);
		NativeMethodInfoPtr_set_Peak_Private_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, 100663479);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, 100663480);
		NativeMethodInfoPtr_Obtain_Public_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, 100663481);
		NativeMethodInfoPtr_Free_Public_Void_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, 100663482);
		NativeMethodInfoPtr_Reset_Protected_Void_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr, 100663483);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 783025, RefRangeEnd = 783030, XrefRangeStart = 783021, XrefRangeEnd = 783025, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Pool(int initialCapacity = 16, int max = int.MaxValue)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Pool<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&initialCapacity);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 783031, RefRangeEnd = 783036, XrefRangeStart = 783030, XrefRangeEnd = 783031, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T Obtain()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Obtain_Public_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 783048, RefRangeEnd = 783055, XrefRangeStart = 783036, XrefRangeEnd = 783048, MetadataInitTokenRva = 46306044L, MetadataInitFlagRva = 59958822L)]
	public unsafe void Free(T obj)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			T val = obj;
			if (val is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
				reference = ref *(_003F*)intPtr;
				if (intPtr != (System.IntPtr)0)
				{
					reference = ref *(_003F*)intPtr;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
					}
				}
			}
		}
		else
		{
			reference = ref obj;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Free_Public_Void_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783055, XrefRangeEnd = 783059, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Reset(T obj)
	{
		//IL_0061->IL0064: Incompatible stack types: I vs Ref
		//IL_0040->IL0064: Incompatible stack types: I vs Ref
		//IL_004d->IL0064: Incompatible stack types: I vs Ref
		//IL_0054->IL0064: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			T val = obj;
			if (val is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
				reference = ref *(_003F*)intPtr;
				if (intPtr != (System.IntPtr)0)
				{
					reference = ref *(_003F*)intPtr;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
					}
				}
			}
		}
		else
		{
			reference = ref obj;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Protected_Void_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
	}

	public Pool(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
