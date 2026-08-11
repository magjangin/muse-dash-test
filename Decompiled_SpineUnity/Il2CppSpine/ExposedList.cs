using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;

namespace Il2CppSpine;

public class ExposedList<T> : Il2CppSystem.Object
{
	public sealed class Enumerator : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_l;

		private static readonly System.IntPtr NativeFieldInfoPtr_next;

		private static readonly System.IntPtr NativeFieldInfoPtr_ver;

		private static readonly System.IntPtr NativeFieldInfoPtr_current;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Internal_Void_ExposedList_1_T_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_VerifyState_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Public_Virtual_Final_New_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Current_Public_Virtual_Final_New_get_T_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;

		public unsafe ExposedList<T> l
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_l);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<T>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_l)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
			}
		}

		public unsafe int next
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_next);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_next)) = num;
			}
		}

		public unsafe int ver
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ver);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ver)) = num;
			}
		}

		public unsafe T current
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_current);
				return IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num, true, false);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_current);
				System.Type typeFromHandle = typeof(T);
				if (!typeFromHandle.IsValueType)
				{
					if (!string.Equals(typeFromHandle.FullName, "System.String"))
					{
						System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((((object)val) is Il2CppObjectBase) ? ((object)val) : null));
						if (intPtr != (System.IntPtr)0 && IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
						{
							System.IntPtr intPtr2 = intPtr;
							// IL cpblk instruction
							Unsafe.CopyBlock(num2, IL2CPP.il2cpp_object_unbox(intPtr), IL2CPP.il2cpp_class_value_size(IL2CPP.il2cpp_object_get_class(intPtr2), ref *(uint*)null));
						}
						else
						{
							IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, intPtr);
						}
					}
					else
					{
						IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, IL2CPP.ManagedStringToIl2Cpp(val as string));
					}
				}
				else
				{
					Unsafe.Write((void*)num2, val);
				}
			}
		}

		public unsafe virtual T Current
		{
			[CallerCount(11)]
			[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Current_Public_Virtual_Final_New_get_T_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
			}
		}

		public unsafe virtual Il2CppSystem.Object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783806, XrefRangeEnd = 783812, MetadataInitTokenRva = 46297996L, MetadataInitFlagRva = 59958759L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		static Enumerator()
		{
			Il2CppClassPointerStore<Enumerator>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, "Enumerator"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Enumerator>.NativeClassPtr);
			NativeFieldInfoPtr_l = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, "l");
			NativeFieldInfoPtr_next = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, "next");
			NativeFieldInfoPtr_ver = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, "ver");
			NativeFieldInfoPtr_current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, "current");
			NativeMethodInfoPtr__ctor_Internal_Void_ExposedList_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, 100663674);
			NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, 100663675);
			NativeMethodInfoPtr_VerifyState_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, 100663676);
			NativeMethodInfoPtr_MoveNext_Public_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, 100663677);
			NativeMethodInfoPtr_get_Current_Public_Virtual_Final_New_get_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, 100663678);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, 100663679);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Enumerator>.NativeClassPtr, 100663680);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783748, XrefRangeEnd = 783749, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Enumerator(ExposedList<T> l)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Enumerator>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)l);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Internal_Void_ExposedList_1_T_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(27)]
		[CachedScanResults(RefRangeStart = 783750, RefRangeEnd = 783777, XrefRangeStart = 783749, XrefRangeEnd = 783750, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783777, XrefRangeEnd = 783778, MetadataInitTokenRva = 46308428L, MetadataInitFlagRva = 59958758L)]
		public unsafe void VerifyState()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_VerifyState_Private_Void_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 783780, RefRangeEnd = 783804, XrefRangeStart = 783778, XrefRangeEnd = 783780, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Public_Virtual_Final_New_Boolean_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783804, XrefRangeEnd = 783806, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Enumerator(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public Enumerator()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Enumerator>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Items;

	private static readonly System.IntPtr NativeFieldInfoPtr_Count;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultCapacity;

	private static readonly System.IntPtr NativeFieldInfoPtr_EmptyArray;

	private static readonly System.IntPtr NativeFieldInfoPtr_version;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Void_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GrowIfNeeded_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Resize_Public_ExposedList_1_T_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnsureCapacity_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddRange_Public_Void_ExposedList_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Contains_Public_Boolean_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyTo_Public_Void_Il2CppArrayBase_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyTo_Public_Void_Il2CppArrayBase_1_T_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Exists_Public_Boolean_Predicate_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Find_Public_T_Predicate_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckMatch_Private_Static_Void_Predicate_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetIndex_Private_Int32_Int32_Int32_Predicate_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetEnumerator_Public_Enumerator_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IndexOf_Public_Int32_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Shift_Private_Void_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckCollection_Private_Void_IEnumerable_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveAt_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToArray_Public_Il2CppArrayBase_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TrimExcess_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Capacity_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Capacity_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_T__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0;

	public unsafe Il2CppArrayBase<T> Items
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items);
			return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int Count
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Count);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Count)) = num;
		}
	}

	public unsafe static int DefaultCapacity
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultCapacity, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultCapacity, (void*)(&num));
		}
	}

	public unsafe static Il2CppArrayBase<T> EmptyArray
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_EmptyArray, (void*)(&intPtr));
			return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_EmptyArray, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int version
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_version);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_version)) = num;
		}
	}

	public unsafe int Capacity
	{
		[CallerCount(31)]
		[CachedScanResults(RefRangeStart = 54977, RefRangeEnd = 55008, XrefRangeStart = 54977, XrefRangeEnd = 55008, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Capacity_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784101, XrefRangeEnd = 784137, MetadataInitTokenRva = 46363380L, MetadataInitFlagRva = 59958779L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Capacity_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static ExposedList()
	{
		Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "ExposedList`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr);
		NativeFieldInfoPtr_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, "Items");
		NativeFieldInfoPtr_Count = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, "Count");
		NativeFieldInfoPtr_DefaultCapacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, "DefaultCapacity");
		NativeFieldInfoPtr_EmptyArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, "EmptyArray");
		NativeFieldInfoPtr_version = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, "version");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663647);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663648);
		NativeMethodInfoPtr_Add_Public_Void_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663649);
		NativeMethodInfoPtr_GrowIfNeeded_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663650);
		NativeMethodInfoPtr_Resize_Public_ExposedList_1_T_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663651);
		NativeMethodInfoPtr_EnsureCapacity_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663652);
		NativeMethodInfoPtr_AddRange_Public_Void_ExposedList_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663653);
		NativeMethodInfoPtr_Clear_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663654);
		NativeMethodInfoPtr_Contains_Public_Boolean_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663655);
		NativeMethodInfoPtr_CopyTo_Public_Void_Il2CppArrayBase_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663656);
		NativeMethodInfoPtr_CopyTo_Public_Void_Il2CppArrayBase_1_T_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663657);
		NativeMethodInfoPtr_Exists_Public_Boolean_Predicate_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663658);
		NativeMethodInfoPtr_Find_Public_T_Predicate_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663659);
		NativeMethodInfoPtr_CheckMatch_Private_Static_Void_Predicate_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663660);
		NativeMethodInfoPtr_GetIndex_Private_Int32_Int32_Int32_Predicate_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663661);
		NativeMethodInfoPtr_GetEnumerator_Public_Enumerator_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663662);
		NativeMethodInfoPtr_IndexOf_Public_Int32_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663663);
		NativeMethodInfoPtr_Shift_Private_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663664);
		NativeMethodInfoPtr_CheckCollection_Private_Void_IEnumerable_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663665);
		NativeMethodInfoPtr_RemoveAt_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663666);
		NativeMethodInfoPtr_ToArray_Public_Il2CppArrayBase_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663667);
		NativeMethodInfoPtr_TrimExcess_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663668);
		NativeMethodInfoPtr_get_Capacity_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663669);
		NativeMethodInfoPtr_set_Capacity_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663670);
		NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_T__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663671);
		NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr, 100663672);
	}

	[CallerCount(1926)]
	[CachedScanResults(RefRangeStart = 321279, RefRangeEnd = 323205, XrefRangeStart = 321279, XrefRangeEnd = 323205, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ExposedList()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 783848, RefRangeEnd = 783858, XrefRangeStart = 783812, XrefRangeEnd = 783848, MetadataInitTokenRva = 46363240L, MetadataInitFlagRva = 59958792L)]
	public unsafe ExposedList(int capacity)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ExposedList<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&capacity);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(71)]
	[CachedScanResults(RefRangeStart = 783859, RefRangeEnd = 783930, XrefRangeStart = 783858, XrefRangeEnd = 783859, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Add(T item)
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
			object obj = item;
			if (obj is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
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
			reference = ref item;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Void_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783930, XrefRangeEnd = 783935, MetadataInitTokenRva = 46362456L, MetadataInitFlagRva = 59958775L)]
	public unsafe void GrowIfNeeded(int addedCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&addedCount);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GrowIfNeeded_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(20)]
	[CachedScanResults(RefRangeStart = 783937, RefRangeEnd = 783957, XrefRangeStart = 783935, XrefRangeEnd = 783937, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ExposedList<T> Resize(int newSize)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&newSize);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Resize_Public_ExposedList_1_T_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<T>>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 783958, RefRangeEnd = 783963, XrefRangeStart = 783957, XrefRangeEnd = 783958, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EnsureCapacity(int min)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&min);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureCapacity_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 783963, RefRangeEnd = 783965, XrefRangeStart = 783963, XrefRangeEnd = 783963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddRange(ExposedList<T> list)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddRange_Public_Void_ExposedList_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(55)]
	[CachedScanResults(RefRangeStart = 783966, RefRangeEnd = 784021, XrefRangeStart = 783965, XrefRangeEnd = 783966, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Clear(bool clearArray = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&clearArray);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 784021, RefRangeEnd = 784025, XrefRangeStart = 784021, XrefRangeEnd = 784021, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Contains(T item)
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
			object obj = item;
			if (obj is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
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
			reference = ref item;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Contains_Public_Boolean_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784026, RefRangeEnd = 784028, XrefRangeStart = 784025, XrefRangeEnd = 784026, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CopyTo(Il2CppArrayBase<T> array)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyTo_Public_Void_Il2CppArrayBase_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 323247, RefRangeEnd = 323252, XrefRangeStart = 323247, XrefRangeEnd = 323252, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CopyTo(Il2CppArrayBase<T> array, int arrayIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &arrayIndex;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyTo_Public_Void_Il2CppArrayBase_1_T_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 784032, RefRangeEnd = 784033, XrefRangeStart = 784028, XrefRangeEnd = 784032, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Exists(Il2CppSystem.Predicate<T> match)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)match);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Exists_Public_Boolean_Predicate_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 784037, RefRangeEnd = 784041, XrefRangeStart = 784033, XrefRangeEnd = 784037, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T Find(Il2CppSystem.Predicate<T> match)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)match);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Find_Public_T_Predicate_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784041, XrefRangeEnd = 784042, MetadataInitTokenRva = 46362268L, MetadataInitFlagRva = 59958776L)]
	public unsafe static void CheckMatch(Il2CppSystem.Predicate<T> match)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)match);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckMatch_Private_Static_Void_Predicate_1_T_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe int GetIndex(int startIndex, int count, Il2CppSystem.Predicate<T> match)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&startIndex);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &count;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)match);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetIndex_Private_Int32_Int32_Int32_Predicate_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(24)]
	[CachedScanResults(RefRangeStart = 784043, RefRangeEnd = 784067, XrefRangeStart = 784042, XrefRangeEnd = 784043, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Enumerator GetEnumerator()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEnumerator_Public_Enumerator_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new Enumerator(pointer);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784067, RefRangeEnd = 784069, XrefRangeStart = 784067, XrefRangeEnd = 784067, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int IndexOf(T item)
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
			object obj = item;
			if (obj is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
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
			reference = ref item;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IndexOf_Public_Int32_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784069, XrefRangeEnd = 784071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Shift(int start, int delta)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&start);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &delta;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Shift_Private_Void_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784071, XrefRangeEnd = 784072, MetadataInitTokenRva = 46361988L, MetadataInitFlagRva = 59958777L)]
	public unsafe void CheckCollection(IEnumerable<T> collection)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collection);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckCollection_Private_Void_IEnumerable_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784074, RefRangeEnd = 784076, XrefRangeStart = 784072, XrefRangeEnd = 784074, MetadataInitTokenRva = 46362696L, MetadataInitFlagRva = 59958796L)]
	public unsafe void RemoveAt(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveAt_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 784079, RefRangeEnd = 784084, XrefRangeStart = 784076, XrefRangeEnd = 784079, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppArrayBase<T> ToArray()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToArray_Public_Il2CppArrayBase_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(intPtr);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 784091, RefRangeEnd = 784101, XrefRangeStart = 784084, XrefRangeEnd = 784091, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TrimExcess()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TrimExcess_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784137, XrefRangeEnd = 784139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual IEnumerator<T> System_Collections_Generic_IEnumerable_T__GetEnumerator()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_T__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_T_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<T>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual IEnumerator System_Collections_IEnumerable_GetEnumerator()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
	}

	public ExposedList(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
