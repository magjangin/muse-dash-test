using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppSharpJson;

public class JsonDecoder : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_EvalLexer_Private_T_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_EvalLexer_Private_T_T_0, Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__errorMessage_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__parseNumbersAsFloat_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_lexer;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_errorMessage_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_errorMessage_Private_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_parseNumbersAsFloat_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_parseNumbersAsFloat_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Decode_Public_Object_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseObject_Private_IDictionary_2_String_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseArray_Private_IList_1_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseValue_Private_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TriggerError_Private_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EvalLexer_Private_T_T_0;

	public unsafe string _errorMessage_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__errorMessage_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__errorMessage_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool _parseNumbersAsFloat_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parseNumbersAsFloat_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parseNumbersAsFloat_k__BackingField)) = flag;
		}
	}

	public unsafe Lexer lexer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lexer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Lexer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lexer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lexer));
		}
	}

	public unsafe string errorMessage
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_errorMessage_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(16)]
		[CachedScanResults(RefRangeStart = 51089, RefRangeEnd = 51105, XrefRangeStart = 51089, XrefRangeEnd = 51105, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_errorMessage_Private_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool parseNumbersAsFloat
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 62252, RefRangeEnd = 62253, XrefRangeStart = 62252, XrefRangeEnd = 62253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_parseNumbersAsFloat_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_parseNumbersAsFloat_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static JsonDecoder()
	{
		Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "SharpJson", "JsonDecoder");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr);
		NativeFieldInfoPtr__errorMessage_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, "<errorMessage>k__BackingField");
		NativeFieldInfoPtr__parseNumbersAsFloat_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, "<parseNumbersAsFloat>k__BackingField");
		NativeFieldInfoPtr_lexer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, "lexer");
		NativeMethodInfoPtr_get_errorMessage_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663312);
		NativeMethodInfoPtr_set_errorMessage_Private_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663313);
		NativeMethodInfoPtr_get_parseNumbersAsFloat_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663314);
		NativeMethodInfoPtr_set_parseNumbersAsFloat_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663315);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663316);
		NativeMethodInfoPtr_Decode_Public_Object_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663317);
		NativeMethodInfoPtr_ParseObject_Private_IDictionary_2_String_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663318);
		NativeMethodInfoPtr_ParseArray_Private_IList_1_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663319);
		NativeMethodInfoPtr_ParseValue_Private_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663320);
		NativeMethodInfoPtr_TriggerError_Private_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663321);
		NativeMethodInfoPtr_EvalLexer_Private_T_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr, 100663322);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781878, XrefRangeEnd = 781880, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe JsonDecoder()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<JsonDecoder>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781880, XrefRangeEnd = 781894, MetadataInitTokenRva = 46442628L, MetadataInitFlagRva = 59849588L)]
	public unsafe Il2CppSystem.Object Decode(string text)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(text);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Decode_Public_Object_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781894, XrefRangeEnd = 781917, MetadataInitTokenRva = 46442836L, MetadataInitFlagRva = 59849589L)]
	public unsafe IDictionary<string, Il2CppSystem.Object> ParseObject()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseObject_Private_IDictionary_2_String_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IDictionary<string, Il2CppSystem.Object>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781917, XrefRangeEnd = 781935, MetadataInitTokenRva = 46442780L, MetadataInitFlagRva = 59849590L)]
	public unsafe IList<Il2CppSystem.Object> ParseArray()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseArray_Private_IList_1_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IList<Il2CppSystem.Object>>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 781940, RefRangeEnd = 781944, XrefRangeStart = 781935, XrefRangeEnd = 781940, MetadataInitTokenRva = 46442896L, MetadataInitFlagRva = 59849591L)]
	public unsafe Il2CppSystem.Object ParseValue()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseValue_Private_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 781950, RefRangeEnd = 781952, XrefRangeStart = 781944, XrefRangeEnd = 781950, MetadataInitTokenRva = 46442912L, MetadataInitFlagRva = 59849592L)]
	public unsafe void TriggerError(string message)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(message);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TriggerError_Private_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781952, XrefRangeEnd = 781956, MetadataInitTokenRva = 46442660L, MetadataInitFlagRva = 59946225L)]
	public unsafe T EvalLexer<T>(T value)
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
			object obj = value;
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
			reference = ref value;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_EvalLexer_Private_T_T_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
	}

	public JsonDecoder(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
