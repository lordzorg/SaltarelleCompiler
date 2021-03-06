﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using QUnit;

namespace CoreLib.TestScript {
	[TestFixture]
	public class ReflectionTests {
		class A1Attribute : Attribute {
			public int X { get; private set; }
			public A1Attribute() {}
			public A1Attribute(int x) { X = x; }
		}

		[NonScriptable]
		class A2Attribute : Attribute {}

		class A3Attribute : Attribute {}

		class A4Attribute : Attribute {}

		public class C1 {
			public void M1() {}
			[A1]
			public void M2() {}
			[Reflectable]
			public void M3() {}
			[A2]
			public void M4() {}
		}

		public class C2 {
			[Reflectable] public void M1() {}
			[Reflectable] public static void M2() {}
		}

		public class C3 {
			[Reflectable] public int M1() { return 0; }
			[Reflectable] public int M2(string x) { return 0; }
			[Reflectable] public int M3(string x, int y) { return 0; }
			[Reflectable] public void M4() {}
		}

		public class C4 {
			[Reflectable] public void M() {}
			[Reflectable] public void M(int i) {}
			[Reflectable, ScriptName("x")] public void M(int i, string s) {}
		}

		public class C5<T1, T2> {
			[Reflectable] public T1 M(T2 t2, string s) { return default(T1); }
			[Reflectable] public object M2() { return null; }
		}

		public class C6 {
			[Reflectable] public T1 M1<T1, T2>(T2 t2, string s) { return default(T1); }
			[Reflectable] public T1 M2<T1>(string s) { return default(T1); }
			[Reflectable] public void M3(string s) {}
		}

		[Serializable]
		public class C7 {
			public int x;
			[Reflectable] public int M1(int x) { return this.x + x; }
			[Reflectable] public static void M2(string x) {}
			[Reflectable] public string M3<T1, T2>(string s) { return x.ToString() + " "  + typeof(T1).FullName + " " + typeof(T2).FullName + " " + s; }
		}

		public class C8 {
			private string s;
			public C8(string s) {
				this.s = s;
			}
			[Reflectable] public string M1(string a, string b) { return s + " " + a + " " + b; }
			[Reflectable] public static string M2(string a, string b) { return a + " " + b; }
			[Reflectable] public string M3<T1, T2>(string a) { return s + " " + typeof(T1).FullName + " " + typeof(T2).FullName + " " + a; }
			[Reflectable] public static string M4<T1, T2>(string a) { return typeof(T1).FullName + " " + typeof(T2).FullName + " " + a; }
		}

		public class C9<T1, T2> {
			[Reflectable] public static string M(string a) { return typeof(T1).FullName + " " + typeof(T2).FullName + " " + a; }
		}

		public class C10 {
			public int X;
			public string S;

			[Reflectable, ScriptName("")] public C10(int x) { X = x; S = "X"; }
			[Reflectable, ScriptName("ctor1")] public C10(int x, string s) { X = x; S = s; }
		}

		[Serializable]
		public class C11 {
			public DateTime D;
			[Reflectable] public C11(DateTime dt) { D = dt; }
		}

		public class C12 {
			[Reflectable] public int F1;
			[Reflectable] public DateTime F2;
			[Reflectable] public static string F3;
		}

		public class C13 {
			[Reflectable] public event Action E1;
			[Reflectable] public static event Action E2;

			public void RaiseE1() { if (E1 != null) E1(); }
			public static void RaiseE2() { if (E2 != null) E2(); }
		}

		public class C14 {
			[Reflectable] public int P1 { get; set; }
			[Reflectable, IntrinsicProperty] public string P2 { get; set; }
			[Reflectable] public static DateTime P3 { get; set; }
			[Reflectable, IntrinsicProperty] public static double P4 { get; set; }

			[Reflectable] public int P5 { get { return 0; } }
			[Reflectable, IntrinsicProperty] public string P6 { get { return null; } }
			[Reflectable] public static DateTime P7 { get { return default(DateTime); } }
			[Reflectable, IntrinsicProperty] public static double P8 { get { return 0; } }

			[Reflectable] public int P9 { set {} }
			[Reflectable, IntrinsicProperty] public string P10 { set {} }
			[Reflectable] public static DateTime P11 { set {} }
			[Reflectable, IntrinsicProperty] public static double P12 { set {} }
		}

		public class C15 {
			public int x;
			public string s;
			public string v;
			[Reflectable] public string this[int x, string s] { get { return v + " " + x + " " + s; } set { this.x = x; this.s = s; this.v = value; } }
		}

		public class C16 {
			[Reflectable] public string this[int x, string s] { get { return null; } }
		}

		public class C17 {
			[Reflectable] public string this[int x, string s] { set {} }
		}

		public class C18 {
			[A1(1), A3] public C18() {}
			[A1(2), A3] public void M() {}
			[A1(3), A3] public int F;
			[A1(4), A3] public int P { [A1(5), A3] get; [A1(6), A3] set; }
			[A1(7), A3] public event Action E { [A1(8), A3] add {} [A1(9), A3] remove {} }
		}

		private ConstructorInfo GetConstructor(Type type) {
			return (ConstructorInfo)type.GetMembers(BindingFlags.Default).Filter(m => m.Name == ".ctor")[0];
		}

		private MethodInfo GetMethod(Type type, string name, BindingFlags flags = BindingFlags.Default) {
			return (MethodInfo)type.GetMembers(flags).Filter(m => m.Name == name)[0];
		}

		private FieldInfo GetField(Type type, string name, BindingFlags flags = BindingFlags.Default) {
			return (FieldInfo)type.GetMembers(flags).Filter(m => m.Name == name)[0];
		}

		private EventInfo GetEvent(Type type, string name, BindingFlags flags = BindingFlags.Default) {
			return (EventInfo)type.GetMembers(flags).Filter(m => m.Name == name)[0];
		}

		private PropertyInfo GetProperty(Type type, string name, BindingFlags flags = BindingFlags.Default) {
			return (PropertyInfo)type.GetMembers(flags).Filter(m => m.Name == name)[0];
		}

		[Test]
		public void GetMembersReturnsMethodsWithAnyScriptableAttributeOrReflectableAttribute() {
			var methods = typeof(C1).GetMembers(BindingFlags.Default);
			Assert.AreEqual(methods.Length, 2, "Should be two methods");
			Assert.IsTrue(methods[0].Name == "M2" || methods[1].Name == "M2");
			Assert.IsTrue(methods[0].Name == "M3" || methods[1].Name == "M3");
		}

		[Test]
		public void GetMemberBindingFlagsInstanceOrStaticWorks() {
			var methods = typeof(C2).GetMembers(BindingFlags.Default);
			Assert.AreEqual(methods.Length, 2, "Default should be two methods");
			Assert.IsTrue(methods[0].Name == "M1" || methods[1].Name == "M1", "M1 should be returned with default binding flags");
			Assert.IsTrue(methods[0].Name == "M2" || methods[1].Name == "M2", "M2 should be returned with default binding flags");

			methods = typeof(C2).GetMembers(BindingFlags.Instance | BindingFlags.Static);
			Assert.AreEqual(methods.Length, 2, "Instance and static should be two methods");
			Assert.IsTrue(methods[0].Name == "M1" || methods[1].Name == "M1", "M1 should be returned with both instance and static binding flags");
			Assert.IsTrue(methods[0].Name == "M2" || methods[1].Name == "M2", "M2 should be returned with both instance and static binding flags");

			methods = typeof(C2).GetMembers(BindingFlags.Instance);
			Assert.AreEqual(methods.Length, 1, "Should be one instance method");
			Assert.IsTrue(methods[0].Name == "M1" || methods[1].Name == "M1", "Instance method should be m1");

			methods = typeof(C2).GetMembers(BindingFlags.Static);
			Assert.AreEqual(methods.Length, 1, "Should be one static method");
			Assert.IsTrue(methods[0].Name == "M2", "Static method should be M2");
		}

		[Test]
		public void IsStaticFlagWorksForMethod() {
			Assert.AreStrictEqual(((MethodInfo)typeof(C2).GetMembers(BindingFlags.Instance)[0]).IsStatic, false, "Instance member should not be static");
			Assert.AreStrictEqual(((MethodInfo)typeof(C2).GetMembers(BindingFlags.Static)[0]).IsStatic, true, "Static member should be static");
		}

		[Test]
		public void MemberTypeIsMethodForMethod() {
			Assert.AreStrictEqual(GetMethod(typeof(C3), "M1").MemberType, MemberTypes.Method);
		}

		[Test]
		public void IsConstructorIsFalseForMethod() {
			Assert.AreStrictEqual(GetMethod(typeof(C3), "M1").IsConstructor, false);
		}

		[Test]
		public void IsConstructorIsTrueForAllKindsOfConstructors() {
			var c10 = typeof(C10).GetMembers();
			var c11 = typeof(C11).GetMembers();
			Assert.IsTrue(((ConstructorInfo)c10[0]).IsConstructor, "Unnamed");
			Assert.IsTrue(((ConstructorInfo)c10[1]).IsConstructor, "Named");
			Assert.IsTrue(((ConstructorInfo)c11[0]).IsConstructor, "Static method");
		}

		[Test]
		public void IsStaticIsFalseForAllKindsOfConstructors() {
			var c10 = typeof(C10).GetMembers();
			var c11 = typeof(C11).GetMembers();
			Assert.IsTrue(((ConstructorInfo)c10[0]).IsConstructor, "Unnamed");
			Assert.IsTrue(((ConstructorInfo)c10[1]).IsConstructor, "Named");
			Assert.IsTrue(((ConstructorInfo)c11[0]).IsConstructor, "Static method");
		}

		[Test]
		public void MemberTypeIsConstructorForAllKindsOfConstructors() {
			var c10 = typeof(C10).GetMembers();
			var c11 = typeof(C11).GetMembers();
			Assert.AreEqual(c10[0].MemberType, MemberTypes.Constructor, "Unnamed");
			Assert.AreEqual(c10[1].MemberType, MemberTypes.Constructor, "Named");
			Assert.AreEqual(c11[0].MemberType, MemberTypes.Constructor, "Static method");
		}

		[Test]
		public void NameIsCtorForAllKindsOfConstructors() {
			var c10 = typeof(C10).GetMembers();
			var c11 = typeof(C11).GetMembers();
			Assert.AreEqual(c10[0].Name, ".ctor", "Unnamed");
			Assert.AreEqual(c10[1].Name, ".ctor", "Named");
			Assert.AreEqual(c11[0].Name, ".ctor", "Static method");
		}

		[Test]
		public void DeclaringTypeIsCorrectForAllKindsOfConstructors() {
			var c10 = typeof(C10).GetMembers();
			var c11 = typeof(C11).GetMembers();
			Assert.AreEqual(c10[0].DeclaringType, typeof(C10), "Unnamed");
			Assert.AreEqual(c10[1].DeclaringType, typeof(C10), "Named");
			Assert.AreEqual(c11[0].DeclaringType, typeof(C11), "Static method");
		}

		[Test]
		public void DeclaringTypeShouldBeCorrectForMethods() {
			Assert.AreStrictEqual(GetMethod(typeof(C3), "M1").DeclaringType, typeof(C3), "Simple type");
			Assert.AreStrictEqual(GetMethod(typeof(C5<,>), "M").DeclaringType, typeof(C5<,>), "Open generic type");
			Assert.AreStrictEqual(GetMethod(typeof(C5<int,string>), "M").DeclaringType, typeof(C5<int,string>), "Constructed generic type");
		}

		[Test]
		public void ReturnTypeAndParameterTypesAreCorrectForMethods() {
			var m1 = GetMethod(typeof(C3), "M1");
			Assert.AreEqual(m1.ReturnType, typeof(int), "Return type should be int");
			Assert.AreEqual(m1.ParameterTypes.Length, 0, "M1 should have no parameters");

			var m2 = GetMethod(typeof(C3), "M2");
			Assert.AreEqual(m2.ParameterTypes, new[] { typeof(string) }, "M2 parameter types should be correct");

			var m3 = GetMethod(typeof(C3), "M3");
			Assert.AreEqual(m3.ParameterTypes, new[] { typeof(string), typeof(int) }, "M3 parameter types should be correct");

			var m4 = GetMethod(typeof(C7), "M1");
			Assert.IsFalse(m4.IsStatic, "M4 should not be static");
			Assert.AreEqual(m4.ParameterTypes, new[] { typeof(int) }, "C7.M1 parameters should be correct");
		}

		[Test]
		public void ParameterTypesShouldBeCorrectForConstructors() {
			var c10 = typeof(C10).GetMembers();
			var c11 = typeof(C11).GetMembers();
			Assert.AreEqual(((ConstructorInfo)c10[0]).ParameterTypes, new[] { typeof(int) }, "Unnamed");
			Assert.AreEqual(((ConstructorInfo)c10[1]).ParameterTypes, new[] { typeof(int), typeof(string) }, "Named");
			Assert.AreEqual(((ConstructorInfo)c11[0]).ParameterTypes, new[] { typeof(DateTime) }, "Static method");
		}

		[Test]
		public void VoidIsConsideredObjectAsReturnType() {
			Assert.AreStrictEqual(GetMethod(typeof(C3), "M4").ReturnType, typeof(object), "Return type of void method should be object");
		}

		[Test]
		public void MethodNameIsTheCSharp() {
			var members = (MethodInfo[])typeof(C4).GetMembers();
			Assert.AreEqual(members.Filter(m => m.Name == "M").Length, 3, "All methods should have name M");
		}

		[Test]
		public void TypeParametersAreReplacedWithObjectForReturnAndParameterTypesForOpenGenericTypes() {
			var m = GetMethod(typeof(C5<,>), "M");
			Assert.AreEqual(m.ReturnType, typeof(object), "Return type should be object");
			Assert.AreEqual(m.ParameterTypes, new[] { typeof(object), typeof(string) }, "Parameters should be correct");
		}

		[Test]
		public void TypeParametersAreCorrectForReturnAndParameterTypesForConstructedGenericTypes() {
			var m = GetMethod(typeof(C5<string,DateTime>), "M");
			Assert.AreEqual(m.ReturnType, typeof(string), "Return type of M should be string");
			Assert.AreEqual(m.ParameterTypes, new[] { typeof(DateTime), typeof(string) }, "Parameters to M should be correct");

			var m2 = GetMethod(typeof(C5<string,DateTime>), "M2");
			Assert.AreEqual(m2.ReturnType, typeof(object), "Return type of M2 should be object");
			Assert.AreEqual(m2.ParameterTypes.Length, 0, "M2 should not have any parameters");
		}

		[Test]
		public void MethodTypeParametersAreReplacedWithObjectForReturnAndParameterTypes() {
			var m = GetMethod(typeof(C6), "M1");
			Assert.AreEqual(m.ReturnType, typeof(object), "Return type should be object");
			Assert.AreEqual(m.ParameterTypes, new[] { typeof(object), typeof(string) }, "Parameters should be correct");
		}

		[Test]
		public void IsGenericMethodDefinitionAndTypeParameterCountWork() {
			Assert.IsTrue (GetMethod(typeof(C6), "M1").IsGenericMethodDefinition, "M1 should be generic");
			Assert.IsTrue (GetMethod(typeof(C6), "M2").IsGenericMethodDefinition, "M2 should be generic");
			Assert.IsFalse(GetMethod(typeof(C6), "M3").IsGenericMethodDefinition, "M3 should not be generic");
			Assert.AreStrictEqual(GetMethod(typeof(C6), "M1").TypeParameterCount, 2, "M1 should have 2 type parameters");
			Assert.AreStrictEqual(GetMethod(typeof(C6), "M2").TypeParameterCount, 1, "M2 should have 1 type parameters");
			Assert.AreStrictEqual(GetMethod(typeof(C6), "M3").TypeParameterCount, 0, "M3 should have 0 type parameters");
		}

		[Test]
		public void CreateDelegateWorksForNonGenericInstanceMethods() {
			var m = GetMethod(typeof(C8), "M1");
			var c = new C8("X");
			var f1 = (Func<string, string, string>)m.CreateDelegate(typeof(Func<string, string, string>), c);
			var f2 = (Func<string, string, string>)m.CreateDelegate(c);
			Assert.AreEqual(f1("a", "b"), "X a b", "Delegate created with delegate type should be correct");
			Assert.AreEqual(f2("c", "d"), "X c d", "Delegate created without delegate type should be correct");
			Assert.Throws(() => m.CreateDelegate(typeof(Func<string, string, string>)), "Without target with delegate type should throw");
			Assert.Throws(() => m.CreateDelegate(), "Without target without delegate type should throw");
			Assert.Throws(() => m.CreateDelegate(typeof(Func<string, string, string>), (object)null), "Null target with delegate type should throw");
			Assert.Throws(() => m.CreateDelegate((object)null), "Null target without delegate type should throw");
			Assert.Throws(() => m.CreateDelegate(c, new[] { typeof(string) }), "With type arguments with target should throw");
			Assert.Throws(() => m.CreateDelegate(new[] { typeof(string) }), "With type arguments without target should throw");
			Assert.Throws(() => m.CreateDelegate((object)null, new[] { typeof(string) }), "With type arguments with null target should throw");
		}

		[Test]
		public void CreateDelegateWorksNonGenericStaticMethods() {
			var m = GetMethod(typeof(C8), "M2");
			var f1 = (Func<string, string, string>)m.CreateDelegate(typeof(Func<string, string, string>));
			var f2 = (Func<string, string, string>)m.CreateDelegate();
			var f3 = (Func<string, string, string>)m.CreateDelegate(typeof(Func<string, string, string>), (object)null);
			var f4 = (Func<string, string, string>)m.CreateDelegate((object)null);
			Assert.AreEqual(f1("a", "b"), "a b", "Delegate created with delegate type without target should be correct");
			Assert.AreEqual(f2("c", "d"), "c d", "Delegate created without delegate type without target should be correct");
			Assert.AreEqual(f3("e", "f"), "e f", "Delegate created with delegate type with null target should be correct");
			Assert.AreEqual(f4("g", "h"), "g h", "Delegate created without delegate type with null target should be correct");
			Assert.Throws(() => m.CreateDelegate(typeof(Func<string, string, string>), new C8("")), "With target with delegate type should throw");
			Assert.Throws(() => m.CreateDelegate(new C8("")), "With target without delegate type should throw");
			Assert.Throws(() => m.CreateDelegate(new C8(""), new[] { typeof(string) }), "With type arguments with target should throw");
			Assert.Throws(() => m.CreateDelegate(new[] { typeof(string) }), "With type arguments without target should throw");
			Assert.Throws(() => m.CreateDelegate((object)null, new[] { typeof(string) }), "With type arguments with null target should throw");
		}

		[Test]
		public void CreateDelegateWorksNonGenericStaticMethodOfGenericType() {
			var m = GetMethod(typeof(C9<int, string>), "M");
			var f = (Func<string, string>)m.CreateDelegate();
			Assert.AreEqual(f("a"), "ss.Int32 String a", "Delegate should return correct results");
		}

		[Test]
		public void CreateDelegateWorksForGenericInstanceMethods() {
			var m = GetMethod(typeof(C8), "M3");
			var c = new C8("X");
			var f = (Func<string, string>)m.CreateDelegate(c, new[] { typeof(int), typeof(string) });
			Assert.AreEqual(f("a"), "X ss.Int32 String a", "Result of invoking delegate should be correct");
			Assert.Throws(() => m.CreateDelegate((object)null, new[] { typeof(int), typeof(string) }), "Null target with correct type arguments should throw");
			Assert.Throws(() => m.CreateDelegate(c), "No type arguments with target should throw");
			Assert.Throws(() => m.CreateDelegate(c, new Type[0]), "0 type arguments with target should throw");
			Assert.Throws(() => m.CreateDelegate(c, new Type[1]), "1 type arguments with target should throw");
			Assert.Throws(() => m.CreateDelegate(c, new Type[3]), "3 type arguments with target should throw");
		}

		[Test]
		public void CreateDelegateWorksForGenericStaticMethods() {
			var m = GetMethod(typeof(C8), "M4");
			var f = (Func<string, string>)m.CreateDelegate((object)null, new[] { typeof(int), typeof(string) });
			Assert.AreEqual(f("a"), "ss.Int32 String a", "Result of invoking delegate should be correct");
			Assert.Throws(() => m.CreateDelegate(new C8(""), new[] { typeof(int), typeof(string) }), "Target with correct type arguments should throw");
			Assert.Throws(() => m.CreateDelegate((object)null), "No type arguments without target should throw");
			Assert.Throws(() => m.CreateDelegate((object)null, new Type[0]), "0 type arguments without target should throw");
			Assert.Throws(() => m.CreateDelegate((object)null, new Type[1]), "1 type arguments without target should throw");
			Assert.Throws(() => m.CreateDelegate((object)null, new Type[3]), "3 type arguments without target should throw");
		}

		[Test]
		public void InvokeWorksForNonGenericInstanceMethods() {
			var m = GetMethod(typeof(C8), "M1");
			var c = new C8("X");
			Assert.AreEqual(m.Invoke(c, "a", "b"), "X a b", "Invoke with target should work");
			Assert.Throws(() => m.Invoke(null, "a", "b"), "Invoke without target should throw");
			Assert.Throws(() => m.Invoke(c, new[] { typeof(string) }, "a", "b"), "Invoke with type arguments with target should throw");
			Assert.Throws(() => m.Invoke(null, new[] { typeof(string) }, "a", "b"), "Invoke with type arguments without target should throw");
		}

		[Test]
		public void InvokeWorksForNonGenericStaticMethods() {
			var m = GetMethod(typeof(C8), "M2");
			Assert.AreEqual(m.Invoke(null, "a", "b"), "a b", "Invoke without target should work");
			Assert.Throws(() => m.Invoke(new C8(""), "a", "b"), "Invoke with target should throw");
			Assert.Throws(() => m.Invoke(new C8(""), new[] { typeof(string) }, "a", "b"), "Invoke with type arguments with target should throw");
			Assert.Throws(() => m.Invoke(null, new[] { typeof(string) }, "a", "b"), "Invoke with type arguments without target should throw");
		}

		[Test]
		public void InvokeWorksForNonGenericInstanceMethodsOnSerializableTypes() {
			var m = GetMethod(typeof(C7), "M1");
			Assert.AreEqual(m.Invoke(new C7 { x = 13 }, 14), 27, "Invoke should work");
		}

		[Test]
		public void InvokeWorksForGenericInstanceMethod() {
			var m = GetMethod(typeof(C8), "M3");
			var c = new C8("X");
			Assert.AreEqual(m.Invoke(c, new[] { typeof(int), typeof(string) }, "a"), "X ss.Int32 String a", "Result of invoking delegate should be correct");
			Assert.Throws(() => m.Invoke(null, new[] { typeof(int), typeof(string) }, "a"), "Null target with correct type arguments should throw");
			Assert.Throws(() => m.Invoke(c, "a"), "No type arguments with target should throw");
			Assert.Throws(() => m.Invoke(c, new Type[0], "a"), "0 type arguments with target should throw");
			Assert.Throws(() => m.Invoke(c, new Type[1], "a"), "1 type arguments with target should throw");
			Assert.Throws(() => m.Invoke(c, new Type[3], "a"), "3 type arguments with target should throw");
		}

		[Test]
		public void InvokeWorksForGenericStaticMethod() {
			var m = GetMethod(typeof(C8), "M4");
			Assert.AreEqual(m.Invoke(null, new[] { typeof(int), typeof(string) }, "a"), "ss.Int32 String a", "Result of invoking delegate should be correct");
			Assert.Throws(() => m.Invoke(new C8(""), new[] { typeof(int), typeof(string) }, "a"), "Target with correct type arguments should throw");
			Assert.Throws(() => m.Invoke(null, "a"), "No type arguments without target should throw");
			Assert.Throws(() => m.Invoke(null, new Type[0], "a"), "0 type arguments without target should throw");
			Assert.Throws(() => m.Invoke(null, new Type[1], "a"), "1 type arguments without target should throw");
			Assert.Throws(() => m.Invoke(null, new Type[3], "a"), "3 type arguments without target should throw");
		}

		[Test]
		public void InvokeWorksForGenericInstanceMethodsOnSerializableTypes() {
			var m = GetMethod(typeof(C7), "M3");
			Assert.AreEqual(m.Invoke(new C7 { x = 13 }, new[] { typeof(int), typeof(string) }, "Suffix"), "13 ss.Int32 String Suffix", "Invoke should work");
		}

		[Test]
		public void InvokeWorksForAllKindsOfConstructors() {
			var c1 = (ConstructorInfo)typeof(C10).GetMembers().Filter(m => ((ConstructorInfo)m).ParameterTypes.Length == 1)[0];
			var o1 = (C10)c1.Invoke(42);
			Assert.AreEqual(o1.X, 42, "o1.X");
			Assert.AreEqual(o1.S, "X", "o1.S");

			var c2 = (ConstructorInfo)typeof(C10).GetMembers().Filter(m => ((ConstructorInfo)m).ParameterTypes.Length == 2)[0];
			var o2 = (C10)c2.Invoke(14, "Hello");
			Assert.AreEqual(o2.X, 14, "o2.X");
			Assert.AreEqual(o2.S, "Hello", "o2.S");

			var c3 = (ConstructorInfo)typeof(C11).GetMembers()[0];
			var o3 = (C11)c3.Invoke(new DateTime(2012, 1, 2));
			Assert.AreEqual(o3.D, new DateTime(2012, 1, 2), "o3.D");
		}

		[Test]
		public void MemberTypeIsFieldForField() {
			Assert.AreStrictEqual(GetField(typeof(C12), "F1").MemberType, MemberTypes.Field, "Instance");
			Assert.AreStrictEqual(GetField(typeof(C12), "F3").MemberType, MemberTypes.Field, "Static");
		}

		[Test]
		public void DeclaringTypeIsCorrectForField() {
			Assert.AreStrictEqual(GetField(typeof(C12), "F1").DeclaringType, typeof(C12), "Instance");
			Assert.AreStrictEqual(GetField(typeof(C12), "F3").DeclaringType, typeof(C12), "Static");
		}

		[Test]
		public void NameIsCorrectForField() {
			Assert.AreStrictEqual(GetField(typeof(C12), "F1").Name, "F1", "Instance");
			Assert.AreStrictEqual(GetField(typeof(C12), "F3").Name, "F3", "Static");
		}

		[Test]
		public void IsStaticIsCorrectForField() {
			Assert.AreStrictEqual(GetField(typeof(C12), "F1").IsStatic, false, "Instance 1");
			Assert.AreStrictEqual(GetField(typeof(C12), "F2").IsStatic, false, "Instance 2");
			Assert.AreStrictEqual(GetField(typeof(C12), "F3").IsStatic, true, "Static");
		}

		[Test]
		public void FieldTypeIsCorrectForField() {
			Assert.AreStrictEqual(GetField(typeof(C12), "F1").FieldType, typeof(int), "Instance 1");
			Assert.AreStrictEqual(GetField(typeof(C12), "F2").FieldType, typeof(DateTime), "Instance 2");
			Assert.AreStrictEqual(GetField(typeof(C12), "F3").FieldType, typeof(string), "Static");
		}

		[Test]
		public void GetValueWorksForInstanceField() {
			var c = new C12 { F1 = 42 };
			Assert.AreEqual(GetField(typeof(C12), "F1").GetValue(c), 42);
		}

		[Test]
		public void GetValueWorksForStaticField() {
			C12.F3 = "X_Test";
			Assert.AreEqual(GetField(typeof(C12), "F3").GetValue(null), "X_Test");
		}

		[Test]
		public void SetValueWorksForInstanceField() {
			var c = new C12();
			GetField(typeof(C12), "F1").SetValue(c, 14);
			Assert.AreEqual(c.F1, 14);
		}

		[Test]
		public void SetValueWorksForStaticField() {
			GetField(typeof(C12), "F3").SetValue(null, "Hello, world");
			Assert.AreEqual(C12.F3, "Hello, world");
		}

		[Test]
		public void MemberTypeIsEventForEvent() {
			Assert.AreStrictEqual(GetEvent(typeof(C13), "E1").MemberType, MemberTypes.Event, "Instance");
			Assert.AreStrictEqual(GetEvent(typeof(C13), "E2").MemberType, MemberTypes.Event, "Static");
		}

		[Test]
		public void DeclaringTypeIsCorrectForEvent() {
			Assert.AreStrictEqual(GetEvent(typeof(C13), "E1").DeclaringType, typeof(C13), "Instance");
			Assert.AreStrictEqual(GetEvent(typeof(C13), "E2").DeclaringType, typeof(C13), "Static");
		}

		[Test]
		public void NameIsCorrectForEvent() {
			Assert.AreStrictEqual(GetEvent(typeof(C13), "E1").Name, "E1", "Instance");
			Assert.AreStrictEqual(GetEvent(typeof(C13), "E2").Name, "E2", "Static");
		}

		[Test]
		public void IsStaticIsCorrectForEvent() {
			Assert.AreStrictEqual(GetEvent(typeof(C13), "E1").IsStatic, false, "Instance");
			Assert.AreStrictEqual(GetEvent(typeof(C13), "E2").IsStatic, true, "Static");
		}

		[Test]
		public void AddEventHandlerMethodWorksForInstanceEvent() {
			int i = 0;
			Action handler = () => i++;
			var obj = new C13();
			var e = GetEvent(typeof(C13), "E1");
			e.AddEventHandler(obj, handler);
			obj.RaiseE1();
			Assert.AreEqual(i, 1, "Event should have been raised");
		}

		[Test]
		public void AddEventHandlerMethodWorksForStaticEvent() {
			int i = 0;
			Action handler = () => i++;
			var e = GetEvent(typeof(C13), "E2");
			e.AddEventHandler(null, handler);
			C13.RaiseE2();
			Assert.AreEqual(i, 1, "Event should have been raised");
		}

		[Test]
		public void RemoveEventHandlerMethodWorksForInstanceEvent() {
			int i = 0;
			Action handler = () => i++;
			var obj = new C13();
			obj.E1 += handler;
			obj.RaiseE1();

			GetEvent(typeof(C13), "E1").RemoveEventHandler(obj, handler);
			obj.RaiseE1();

			Assert.AreEqual(i, 1, "Event handler should have been removed");
		}

		[Test]
		public void RemoveEventHandlerMethodWorksForStaticEvent() {
			int i = 0;
			Action handler = () => i++;
			C13.E2 += handler;
			C13.RaiseE2();

			GetEvent(typeof(C13), "E2").RemoveEventHandler(null, handler);
			C13.RaiseE2();

			Assert.AreEqual(i, 1, "Event handler should have been removed");
		}

		[Test]
		public void PropertiesForAddMethodAreCorrect() {
			var m1 = GetEvent(typeof(C13), "E1").AddMethod;
			var m2 = GetEvent(typeof(C13), "E2").AddMethod;

			Assert.AreEqual(m1.MemberType, MemberTypes.Method, "m1.MemberType");
			Assert.AreEqual(m2.MemberType, MemberTypes.Method, "m2.MemberType");
			Assert.AreEqual(m1.Name, "add_E1", "m1.Name");
			Assert.AreEqual(m2.Name, "add_E2", "m2.Name");
			Assert.AreEqual(m1.DeclaringType, typeof(C13), "m1.DeclaringType");
			Assert.AreEqual(m2.DeclaringType, typeof(C13), "m2.DeclaringType");
			Assert.IsFalse (m1.IsStatic, "m1.IsStatic");
			Assert.IsTrue  (m2.IsStatic, "m2.IsStatic");
			Assert.AreEqual(m1.ParameterTypes, new[] { typeof(Delegate) }, "m1.ParameterTypes");
			Assert.AreEqual(m2.ParameterTypes, new[] { typeof(Delegate) }, "m2.ParameterTypes");
			Assert.IsFalse (m1.IsConstructor, "m1.IsConstructor");
			Assert.IsFalse (m2.IsConstructor, "m2.IsConstructor");
			Assert.AreEqual(m1.ReturnType, typeof(object), "m1.ReturnType");
			Assert.AreEqual(m2.ReturnType, typeof(object), "m2.ReturnType");
			Assert.AreEqual(m1.TypeParameterCount, 0, "m1.TypeParameterCount");
			Assert.AreEqual(m2.TypeParameterCount, 0, "m2.TypeParameterCount");
			Assert.AreStrictEqual(m1.IsGenericMethodDefinition, false, "m1.IsGenericMethodDefinition");
			Assert.AreStrictEqual(m2.IsGenericMethodDefinition, false, "m2.IsGenericMethodDefinition");

			int i1 = 0, i2 = 0;
			var obj = new C13();
			Action handler1 = () => i1++, handler2 = () => i2++;
			m1.Invoke(obj, handler1);
			obj.RaiseE1();
			Assert.AreEqual(i1, 1, "m1.Invoke");

			m2.Invoke(null, handler2);
			C13.RaiseE2();
			Assert.AreEqual(i2, 1, "m2.Invoke");
		}

		[Test]
		public void PropertiesForRemoveMethodAreCorrect() {
			var m1 = GetEvent(typeof(C13), "E1").RemoveMethod;
			var m2 = GetEvent(typeof(C13), "E2").RemoveMethod;

			Assert.AreEqual(m1.MemberType, MemberTypes.Method, "m1.MemberType");
			Assert.AreEqual(m2.MemberType, MemberTypes.Method, "m2.MemberType");
			Assert.AreEqual(m1.Name, "remove_E1", "m1.Name");
			Assert.AreEqual(m2.Name, "remove_E2", "m2.Name");
			Assert.AreEqual(m1.DeclaringType, typeof(C13), "m1.DeclaringType");
			Assert.AreEqual(m2.DeclaringType, typeof(C13), "m2.DeclaringType");
			Assert.IsFalse (m1.IsStatic, "m1.IsStatic");
			Assert.IsTrue  (m2.IsStatic, "m2.IsStatic");
			Assert.AreEqual(m1.ParameterTypes, new[] { typeof(Delegate) }, "m1.ParameterTypes");
			Assert.AreEqual(m2.ParameterTypes, new[] { typeof(Delegate) }, "m2.ParameterTypes");
			Assert.IsFalse (m1.IsConstructor, "m1.IsConstructor");
			Assert.IsFalse (m2.IsConstructor, "m2.IsConstructor");
			Assert.AreEqual(m1.ReturnType, typeof(object), "m1.ReturnType");
			Assert.AreEqual(m2.ReturnType, typeof(object), "m2.ReturnType");
			Assert.AreEqual(m1.TypeParameterCount, 0, "m1.TypeParameterCount");
			Assert.AreEqual(m2.TypeParameterCount, 0, "m2.TypeParameterCount");
			Assert.AreStrictEqual(m1.IsGenericMethodDefinition, false, "m1.IsGenericMethodDefinition");
			Assert.AreStrictEqual(m2.IsGenericMethodDefinition, false, "m2.IsGenericMethodDefinition");

			int i1 = 0, i2 = 0;
			var obj = new C13();
			Action handler1 = () => i1++, handler2 = () => i2++;
			obj.E1 += handler1;
			m1.Invoke(obj, handler1);
			obj.RaiseE1();
			Assert.AreEqual(i1, 0, "m1.Invoke");

			C13.E2 += handler2;
			m2.Invoke(null, handler2);
			C13.RaiseE2();
			Assert.AreEqual(i2, 0, "m2.Invoke");
		}

		[Test]
		public void MemberTypeIsPropertyForProperty() {
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P1").MemberType, MemberTypes.Property, "P1");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P2").MemberType, MemberTypes.Property, "P2");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P3").MemberType, MemberTypes.Property, "P3");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P4").MemberType, MemberTypes.Property, "P4");
		}

		[Test]
		public void MemberTypeIsPropertyForIndexer() {
			Assert.AreStrictEqual(GetProperty(typeof(C15), "Item").MemberType, MemberTypes.Property);
		}

		[Test]
		public void DeclaringTypeIsCorrectForProperty() {
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P1").DeclaringType, typeof(C14), "P1");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P2").DeclaringType, typeof(C14), "P2");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P3").DeclaringType, typeof(C14), "P3");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P4").DeclaringType, typeof(C14), "P4");
		}

		[Test]
		public void DeclaringTypeIsCorrectForIndexer() {
			Assert.AreStrictEqual(GetProperty(typeof(C15), "Item").DeclaringType, typeof(C15));
		}

		[Test]
		public void NameIsCorrectForProperty() {
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P1").Name, "P1");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P2").Name, "P2");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P3").Name, "P3");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P4").Name, "P4");
		}

		[Test]
		public void NameIsCorrectForIndexer() {
			Assert.AreStrictEqual(GetProperty(typeof(C15), "Item").Name, "Item");
		}

		[Test]
		public void IsStaticIsCorrectForProperty() {
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P1").IsStatic, false, "P1");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P2").IsStatic, false, "P2");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P3").IsStatic, true, "P3");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P4").IsStatic, true, "P4");
		}

		[Test]
		public void IsStaticIsFalseForIndexer() {
			Assert.AreStrictEqual(GetProperty(typeof(C15), "Item").IsStatic, false);
		}

		[Test]
		public void PropertyTypeIsCorrectForProperty() {
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P1").PropertyType, typeof(int), "P1");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P2").PropertyType, typeof(string), "P2");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P3").PropertyType, typeof(DateTime), "P3");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P4").PropertyType, typeof(double), "P4");
		}

		[Test]
		public void PropertyTypeIsCorrectForIndexer() {
			Assert.AreStrictEqual(GetProperty(typeof(C15), "Item").PropertyType, typeof(string));
		}

		[Test]
		public void IndexParameterTypesAreEmptyForProperty() {
			Assert.AreEqual(GetProperty(typeof(C14), "P1").IndexParameterTypes, new Type[0], "P1");
			Assert.AreEqual(GetProperty(typeof(C14), "P2").IndexParameterTypes, new Type[0], "P2");
			Assert.AreEqual(GetProperty(typeof(C14), "P3").IndexParameterTypes, new Type[0], "P3");
			Assert.AreEqual(GetProperty(typeof(C14), "P4").IndexParameterTypes, new Type[0], "P4");
		}

		[Test]
		public void IndexParameterTypesAreCorrectForIndexer() {
			Assert.AreEqual(GetProperty(typeof(C15), "Item").IndexParameterTypes, new[] { typeof(int), typeof(string) });
		}

		[Test]
		public void PropertiesForGetMethodAreCorrectForPropertyImplementedAsGetAndSetMethods() {
			var m1 = GetProperty(typeof(C14), "P1").GetMethod;
			var m2 = GetProperty(typeof(C14), "P3").GetMethod;

			Assert.AreEqual(m1.MemberType, MemberTypes.Method, "m1.MemberType");
			Assert.AreEqual(m2.MemberType, MemberTypes.Method, "m2.MemberType");
			Assert.AreEqual(m1.Name, "get_P1", "m1.Name");
			Assert.AreEqual(m2.Name, "get_P3", "m2.Name");
			Assert.AreEqual(m1.DeclaringType, typeof(C14), "m1.DeclaringType");
			Assert.AreEqual(m2.DeclaringType, typeof(C14), "m2.DeclaringType");
			Assert.IsFalse (m1.IsStatic, "m1.IsStatic");
			Assert.IsTrue  (m2.IsStatic, "m2.IsStatic");
			Assert.AreEqual(m1.ParameterTypes.Length, 0, "m1.ParameterTypes");
			Assert.AreEqual(m2.ParameterTypes.Length, 0, "m2.ParameterTypes");
			Assert.IsFalse (m1.IsConstructor, "m1.IsConstructor");
			Assert.IsFalse (m2.IsConstructor, "m2.IsConstructor");
			Assert.AreEqual(m1.ReturnType, typeof(int), "m1.ReturnType");
			Assert.AreEqual(m2.ReturnType, typeof(DateTime), "m2.ReturnType");
			Assert.AreEqual(m1.TypeParameterCount, 0, "m1.TypeParameterCount");
			Assert.AreEqual(m2.TypeParameterCount, 0, "m2.TypeParameterCount");
			Assert.AreStrictEqual(m1.IsGenericMethodDefinition, false, "m1.IsGenericMethodDefinition");
			Assert.AreStrictEqual(m2.IsGenericMethodDefinition, false, "m2.IsGenericMethodDefinition");

			var c = new C14() { P1 = 78 };
			object p1 = m1.Invoke(c);
			Assert.AreEqual(p1, 78, "m1.Invoke");

			C14.P3 = new DateTime(2012, 4, 2);
			object p2 = m2.Invoke(null);
			Assert.AreEqual(p2, new DateTime(2012, 4, 2), "m2.Invoke");
		}

		[Test]
		public void PropertiesForSetMethodAreCorrectForPropertyImplementedAsGetAndSetMethods() {
			var m1 = GetProperty(typeof(C14), "P1").SetMethod;
			var m2 = GetProperty(typeof(C14), "P3").SetMethod;

			Assert.AreEqual(m1.MemberType, MemberTypes.Method, "m1.MemberType");
			Assert.AreEqual(m2.MemberType, MemberTypes.Method, "m2.MemberType");
			Assert.AreEqual(m1.Name, "set_P1", "m1.Name");
			Assert.AreEqual(m2.Name, "set_P3", "m2.Name");
			Assert.AreEqual(m1.DeclaringType, typeof(C14), "m1.DeclaringType");
			Assert.AreEqual(m2.DeclaringType, typeof(C14), "m2.DeclaringType");
			Assert.IsFalse (m1.IsStatic, "m1.IsStatic");
			Assert.IsTrue  (m2.IsStatic, "m2.IsStatic");
			Assert.AreEqual(m1.ParameterTypes, new[] { typeof(int) }, "m1.ParameterTypes");
			Assert.AreEqual(m2.ParameterTypes, new[] { typeof(DateTime) }, "m2.ParameterTypes");
			Assert.IsFalse (m1.IsConstructor, "m1.IsConstructor");
			Assert.IsFalse (m2.IsConstructor, "m2.IsConstructor");
			Assert.AreEqual(m1.ReturnType, typeof(object), "m1.ReturnType");
			Assert.AreEqual(m2.ReturnType, typeof(object), "m2.ReturnType");
			Assert.AreEqual(m1.TypeParameterCount, 0, "m1.TypeParameterCount");
			Assert.AreEqual(m2.TypeParameterCount, 0, "m2.TypeParameterCount");
			Assert.AreStrictEqual(m1.IsGenericMethodDefinition, false, "m1.IsGenericMethodDefinition");
			Assert.AreStrictEqual(m2.IsGenericMethodDefinition, false, "m2.IsGenericMethodDefinition");

			var c = new C14();
			m1.Invoke(c, 42);
			Assert.AreEqual(c.P1, 42, "m1.Invoke");

			C14.P3 = new DateTime(2010, 1, 1);
			m2.Invoke(null, new DateTime(2012, 2, 3));
			Assert.AreEqual(C14.P3, new DateTime(2012, 2, 3), "m2.Invoke");
		}

		[Test]
		public void PropertiesForGetMethodAreCorrectForPropertyImplementedAsFields() {
			var m1 = GetProperty(typeof(C14), "P2").GetMethod;
			var m2 = GetProperty(typeof(C14), "P4").GetMethod;

			Assert.AreEqual(m1.MemberType, MemberTypes.Method, "m1.MemberType");
			Assert.AreEqual(m2.MemberType, MemberTypes.Method, "m2.MemberType");
			Assert.AreEqual(m1.Name, "get_P2", "m1.Name");
			Assert.AreEqual(m2.Name, "get_P4", "m2.Name");
			Assert.AreEqual(m1.DeclaringType, typeof(C14), "m1.DeclaringType");
			Assert.AreEqual(m2.DeclaringType, typeof(C14), "m2.DeclaringType");
			Assert.IsFalse (m1.IsStatic, "m1.IsStatic");
			Assert.IsTrue  (m2.IsStatic, "m2.IsStatic");
			Assert.AreEqual(m1.ParameterTypes.Length, 0, "m1.ParameterTypes");
			Assert.AreEqual(m2.ParameterTypes.Length, 0, "m2.ParameterTypes");
			Assert.IsFalse (m1.IsConstructor, "m1.IsConstructor");
			Assert.IsFalse (m2.IsConstructor, "m2.IsConstructor");
			Assert.AreEqual(m1.ReturnType, typeof(string), "m1.ReturnType");
			Assert.AreEqual(m2.ReturnType, typeof(double), "m2.ReturnType");
			Assert.AreEqual(m1.TypeParameterCount, 0, "m1.TypeParameterCount");
			Assert.AreEqual(m2.TypeParameterCount, 0, "m2.TypeParameterCount");
			Assert.AreStrictEqual(m1.IsGenericMethodDefinition, false, "m1.IsGenericMethodDefinition");
			Assert.AreStrictEqual(m2.IsGenericMethodDefinition, false, "m2.IsGenericMethodDefinition");

			var c = new C14() { P2 = "Hello, world" };
			object p1 = m1.Invoke(c);
			Assert.AreEqual(p1, "Hello, world", "m1.Invoke");

			C14.P4 = 3.5;
			object p2 = m2.Invoke(null);
			Assert.AreEqual(p2, 3.5, "m2.Invoke");
		}

		[Test]
		public void PropertiesForSetMethodAreCorrectForPropertyImplementedAsFields() {
			var m1 = GetProperty(typeof(C14), "P2").SetMethod;
			var m2 = GetProperty(typeof(C14), "P4").SetMethod;

			Assert.AreEqual(m1.MemberType, MemberTypes.Method, "m1.MemberType");
			Assert.AreEqual(m2.MemberType, MemberTypes.Method, "m2.MemberType");
			Assert.AreEqual(m1.Name, "set_P2", "m1.Name");
			Assert.AreEqual(m2.Name, "set_P4", "m2.Name");
			Assert.AreEqual(m1.DeclaringType, typeof(C14), "m1.DeclaringType");
			Assert.AreEqual(m2.DeclaringType, typeof(C14), "m2.DeclaringType");
			Assert.IsFalse (m1.IsStatic, "m1.IsStatic");
			Assert.IsTrue  (m2.IsStatic, "m2.IsStatic");
			Assert.AreEqual(m1.ParameterTypes, new[] { typeof(string) }, "m1.ParameterTypes");
			Assert.AreEqual(m2.ParameterTypes, new[] { typeof(double) }, "m2.ParameterTypes");
			Assert.IsFalse (m1.IsConstructor, "m1.IsConstructor");
			Assert.IsFalse (m2.IsConstructor, "m2.IsConstructor");
			Assert.AreEqual(m1.ReturnType, typeof(object), "m1.ReturnType");
			Assert.AreEqual(m2.ReturnType, typeof(object), "m2.ReturnType");
			Assert.AreEqual(m1.TypeParameterCount, 0, "m1.TypeParameterCount");
			Assert.AreEqual(m2.TypeParameterCount, 0, "m2.TypeParameterCount");
			Assert.AreStrictEqual(m1.IsGenericMethodDefinition, false, "m1.IsGenericMethodDefinition");
			Assert.AreStrictEqual(m2.IsGenericMethodDefinition, false, "m2.IsGenericMethodDefinition");

			var c = new C14();
			m1.Invoke(c, "Something");
			Assert.AreEqual(c.P2, "Something", "m1.Invoke");

			C14.P4 = 7.5;
			m2.Invoke(null, 2.5);
			Assert.AreEqual(C14.P4, 2.5, "m2.Invoke");
		}

		[Test]
		public void PropertiesForGetMethodAreCorrectForIndexer() {
			var m = GetProperty(typeof(C15), "Item").GetMethod;

			Assert.AreEqual(m.MemberType, MemberTypes.Method, "MemberType");
			Assert.AreEqual(m.Name, "get_Item", "Name");
			Assert.AreEqual(m.DeclaringType, typeof(C15), "DeclaringType");
			Assert.IsFalse (m.IsStatic, "IsStatic");
			Assert.AreEqual(m.ParameterTypes, new[] { typeof(int), typeof(string) }, "ParameterTypes");
			Assert.IsFalse (m.IsConstructor, "IsConstructor");
			Assert.AreEqual(m.ReturnType, typeof(string), "ReturnType");
			Assert.AreEqual(m.TypeParameterCount, 0, "TypeParameterCount");
			Assert.AreStrictEqual(m.IsGenericMethodDefinition, false, "IsGenericMethodDefinition");

			var c = new C15() { v = "X" };
			object v = m.Invoke(c, 42, "Hello");
			Assert.AreEqual(v, "X 42 Hello", "Invoke");
		}

		[Test]
		public void PropertiesForSetMethodAreCorrectForIndexer() {
			var m = GetProperty(typeof(C15), "Item").SetMethod;

			Assert.AreEqual(m.MemberType, MemberTypes.Method, "MemberType");
			Assert.AreEqual(m.Name, "set_Item", "Name");
			Assert.AreEqual(m.DeclaringType, typeof(C15), "DeclaringType");
			Assert.IsFalse (m.IsStatic, "IsStatic");
			Assert.AreEqual(m.ParameterTypes, new[] { typeof(int), typeof(string), typeof(string) }, "ParameterTypes");
			Assert.IsFalse (m.IsConstructor, "IsConstructor");
			Assert.AreEqual(m.ReturnType, typeof(object), "ReturnType");
			Assert.AreEqual(m.TypeParameterCount, 0, "TypeParameterCount");
			Assert.AreStrictEqual(m.IsGenericMethodDefinition, false, "IsGenericMethodDefinition");

			var c = new C15();
			m.Invoke(c, 42, "Hello", "The_value");

			Assert.AreEqual(c.x, 42, "invoke (x)");
			Assert.AreEqual(c.s, "Hello", "invoke (s)");
			Assert.AreEqual(c.v, "The_value", "invoke (value)");
		}

		[Test]
		public void CanReadAndWriteAndPropertiesWithOnlyOneAccessor() {
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P1" ).CanRead,  true,   "P1.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P1" ).CanWrite, true,   "P1.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P2" ).CanRead,  true,   "P2.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P2" ).CanWrite, true,   "P2.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P3" ).CanRead,  true,   "P3.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P3" ).CanWrite, true,   "P3.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P4" ).CanRead,  true,   "P4.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P4" ).CanWrite, true,   "P4.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P5" ).CanRead,  true,   "P5.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P5" ).CanWrite, false,  "P5.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P6" ).CanRead,  true,   "P6.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P6" ).CanWrite, false,  "P6.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P7" ).CanRead,  true,   "P7.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P7" ).CanWrite, false,  "P7.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P8" ).CanRead,  true,   "P8.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P8" ).CanWrite, false,  "P8.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P9" ).CanRead,  false,  "P9.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P9" ).CanWrite, true,   "P9.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P10").CanRead,  false, "P10.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P10").CanWrite, true,  "P10.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P11").CanRead,  false, "P11.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P11").CanWrite, true,  "P11.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P12").CanRead,  false, "P12.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C14), "P12").CanWrite, true,  "P12.CanWrite");

			Assert.IsTrue (GetProperty(typeof(C14), "P1" ).GetMethod != null,  "P1.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P1" ).SetMethod != null,  "P1.SetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P2" ).GetMethod != null,  "P2.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P2" ).SetMethod != null,  "P2.SetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P3" ).GetMethod != null,  "P3.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P3" ).SetMethod != null,  "P3.SetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P4" ).GetMethod != null,  "P4.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P4" ).SetMethod != null,  "P4.SetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P5" ).GetMethod != null,  "P5.GetMethod");
			Assert.IsFalse(GetProperty(typeof(C14), "P5" ).SetMethod != null,  "P5.SetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P6" ).GetMethod != null,  "P6.GetMethod");
			Assert.IsFalse(GetProperty(typeof(C14), "P6" ).SetMethod != null,  "P6.SetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P7" ).GetMethod != null,  "P7.GetMethod");
			Assert.IsFalse(GetProperty(typeof(C14), "P7" ).SetMethod != null,  "P7.SetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P8" ).GetMethod != null,  "P8.GetMethod");
			Assert.IsFalse(GetProperty(typeof(C14), "P8" ).SetMethod != null,  "P8.SetMethod");
			Assert.IsFalse(GetProperty(typeof(C14), "P9" ).GetMethod != null,  "P9.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P9" ).SetMethod != null,  "P9.SetMethod");
			Assert.IsFalse(GetProperty(typeof(C14), "P10").GetMethod != null, "P10.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P10").SetMethod != null, "P10.SetMethod");
			Assert.IsFalse(GetProperty(typeof(C14), "P11").GetMethod != null, "P11.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P11").SetMethod != null, "P11.SetMethod");
			Assert.IsFalse(GetProperty(typeof(C14), "P12").GetMethod != null, "P12.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C14), "P12").SetMethod != null, "P12.SetMethod");
		}

		[Test]
		public void CanReadAndWriteAndIndexersWithOnlyOneAccessor() {
			Assert.AreStrictEqual(GetProperty(typeof(C15), "Item" ).CanRead,  true,  "C15.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C15), "Item" ).CanWrite, true,  "C15.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C16), "Item" ).CanRead,  true,  "C16.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C16), "Item" ).CanWrite, false, "C16.CanWrite");
			Assert.AreStrictEqual(GetProperty(typeof(C17), "Item" ).CanRead,  false, "C17.CanRead");
			Assert.AreStrictEqual(GetProperty(typeof(C17), "Item" ).CanWrite, true,  "C17.CanWrite");

			Assert.IsTrue (GetProperty(typeof(C15), "Item" ).GetMethod != null, "C15.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C15), "Item" ).SetMethod != null, "C15.SetMethod");
			Assert.IsTrue (GetProperty(typeof(C16), "Item" ).GetMethod != null, "C16.GetMethod");
			Assert.IsFalse(GetProperty(typeof(C16), "Item" ).SetMethod != null, "C16.SetMethod");
			Assert.IsFalse(GetProperty(typeof(C17), "Item" ).GetMethod != null, "C17.GetMethod");
			Assert.IsTrue (GetProperty(typeof(C17), "Item" ).SetMethod != null, "C17.SetMethod");
		}

		[Test]
		public void PropertyInfoGetValueWorks() {
			var p1 = GetProperty(typeof(C14), "P1");
			var p2 = GetProperty(typeof(C14), "P2");
			var p3 = GetProperty(typeof(C14), "P3");
			var p4 = GetProperty(typeof(C14), "P4");
			var i  = GetProperty(typeof(C15), "Item");

			var c14 = new C14 { P1 = 42, P2 = "Hello, world!" };
			C14.P3 = new DateTime(2013, 3, 5);
			C14.P4 = 7.5;
			Assert.AreEqual(p1.GetValue(c14), 42, "P1.GetValue");
			Assert.AreEqual(p2.GetValue(c14), "Hello, world!", "P2.GetValue");
			Assert.AreEqual(p3.GetValue(null), new DateTime(2013, 3, 5), "P3.GetValue");
			Assert.AreEqual(p4.GetValue(null), 7.5, "P4.GetValue");

			var c15 = new C15() { v = "X" };
			Assert.AreEqual(i.GetValue(c15, new object[] { 42, "Hello" }), "X 42 Hello", "Item.GetValue");
		}

		[Test]
		public void PropertyInfoSetValueWorks() {
			var p1 = GetProperty(typeof(C14), "P1");
			var p2 = GetProperty(typeof(C14), "P2");
			var p3 = GetProperty(typeof(C14), "P3");
			var p4 = GetProperty(typeof(C14), "P4");
			var i  = GetProperty(typeof(C15), "Item");

			var c14 = new C14();
			p1.SetValue(c14, 42);
			p2.SetValue(c14, "Hello, world!");
			p3.SetValue(null, new DateTime(2013, 3, 5));
			p4.SetValue(null, 7.5);

			Assert.AreEqual(c14.P1, 42, "P1.SetValue");
			Assert.AreEqual(c14.P2, "Hello, world!", "P2.SetValue");
			Assert.AreEqual(C14.P3, new DateTime(2013, 3, 5), "P3.SetValue");
			Assert.AreEqual(C14.P4, 7.5, "P4.SetValue");

			var c15 = new C15() { v = "X" };
			i.SetValue(c15, "The_value", new object[] { 378, "X" });
			Assert.AreEqual(c15.s, "X", "Item.SetValue.s");
			Assert.AreEqual(c15.x, 378, "Item.SetValue.x");
			Assert.AreEqual(c15.v, "The_value", "Item.SetValue.value");
		}

		private void TestMemberAttribute(MemberInfo member, int expectedA1) {
			var all = member.GetCustomAttributes();
			Assert.AreEqual(all.Length, 2);
			Assert.IsTrue(all[0] is A1Attribute || all[1] is A1Attribute);
			Assert.IsTrue(all[0] is A3Attribute || all[1] is A3Attribute);
			Assert.AreEqual(((A1Attribute)(all[0] is A1Attribute ? all[0] : all[1])).X, expectedA1);

			all = member.GetCustomAttributes(true);
			Assert.AreEqual(all.Length, 2);
			Assert.IsTrue(all[0] is A1Attribute || all[1] is A1Attribute);
			Assert.IsTrue(all[0] is A3Attribute || all[1] is A3Attribute);
			Assert.AreEqual(((A1Attribute)(all[0] is A1Attribute ? all[0] : all[1])).X, expectedA1);

			all = member.GetCustomAttributes(typeof(A1Attribute));
			Assert.AreEqual(all.Length, 1);
			Assert.IsTrue(all[0] is A1Attribute);
			Assert.AreEqual(((A1Attribute)all[0]).X, expectedA1);

			all = member.GetCustomAttributes(typeof(A1Attribute), false);
			Assert.AreEqual(all.Length, 1);
			Assert.IsTrue(all[0] is A1Attribute);
			Assert.AreEqual(((A1Attribute)all[0]).X, expectedA1);

			Assert.AreEqual(member.GetCustomAttributes(typeof(A4Attribute)).Length, 0);
			Assert.AreEqual(member.GetCustomAttributes(typeof(A4Attribute), false).Length, 0);
		}

		[Test]
		public void MemberAttributesWork() {
			TestMemberAttribute(GetConstructor(typeof(C18)), 1);
			TestMemberAttribute(GetMethod(typeof(C18), "M"), 2);
			TestMemberAttribute(GetField(typeof(C18), "F"), 3);
			TestMemberAttribute(GetProperty(typeof(C18), "P"), 4);
			TestMemberAttribute(GetProperty(typeof(C18), "P").GetMethod, 5);
			TestMemberAttribute(GetProperty(typeof(C18), "P").SetMethod, 6);
			TestMemberAttribute(GetEvent(typeof(C18), "E"), 7);
			TestMemberAttribute(GetEvent(typeof(C18), "E").AddMethod, 8);
			TestMemberAttribute(GetEvent(typeof(C18), "E").RemoveMethod, 9);

			Assert.AreEqual(GetMethod(typeof(C2), "M1").GetCustomAttributes().Length, 0);
		}
	}
}
