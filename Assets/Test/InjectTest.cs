#region Usings
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using IoC;
#endregion

namespace Test
{
	interface ITestInjection
	{
		int ReturnValue();
	}
	
	class TestInjection:ITestInjection
	{
		public int ReturnValue() { return 5; }
	}
	
	class TestMonoBehaviourInjection:MonoBehaviour, ITestInjection
	{
		public int ReturnValue() { return 5; }
	}
	
	[ExecuteInEditMode] //the tests run in the editor
	class TestMonoBehaviourSelfInjection:MonoBehaviour
	{
		[IoC.Inject] public ITestInjection testInjection { set; get; }
		
		void Awake()
		{
			this.Inject();
		}
	}
	
	class TestContext:IContextRoot
	{
		public IoC.IContainer container { get; private set; }
		
		public TestContext()
		{
			container = new IoC.UnityContainer();
			
			container.Bind<ITestInjection>().AsSingle<TestInjection>();
		}
	}
	
	[ExecuteInEditMode] //the tests run in the editor
	class TestContextMB:UnityContext<TestContext>
	{
	}
	
	[TestFixture]
    public class TaskRunnerTests
	{
		IContainer		_applicationContainer;
		GameObject		_go;
		
		[SetUp]
		public void Setup()
		{
			_applicationContainer = new UnityContainer();
			
			_go = new GameObject("TestInjection");
		}
		
		[TearDown]
		public void Destroy()
		{
			GameObject.DestroyImmediate(_go);
		}
		
		[Test]
		public void TestInterfaceBoundToImplementationRegistration()
		{
			_applicationContainer.Bind<ITestInjection>().AsSingle<TestInjection>();
			
			Assert.That(_applicationContainer.Build<ITestInjection>().ReturnValue() == 5);
		}
		
		[Test]
		public void TestClassRegistration()
		{
			_applicationContainer.Bind<TestInjection>().AsSingle();
			
			Assert.That(_applicationContainer.Build<TestInjection>().ReturnValue() == 5);
		}
		
		[Test]
		public void TestInterfaceBoundToInstanceRegistration()
		{
			ITestInjection instance = new TestInjection();
			
			_applicationContainer.Bind<ITestInjection>().AsSingle(instance);
			
			Assert.That(_applicationContainer.Build<ITestInjection>().ReturnValue() == 5);
		}
		
		[Test]
		public void TestInterfaceBoundToMonoBehaviour()
		{
			MonoBehaviour mb = _go.AddComponent<TestMonoBehaviourInjection>();
			
			_applicationContainer.Bind<ITestInjection>().AsSingle((ITestInjection)mb);
		 	
			Assert.That(_applicationContainer.Build<ITestInjection>().ReturnValue() == 5);
		}
		
		[Test]
		public void TestMonoBehaviourSelfInjection()
		{
			_go.AddComponent<TestContextMB>();
			
			GameObject childGo = new GameObject("childTest");
			
			childGo.transform.parent = _go.transform;
			 
			MonoBehaviour mb = childGo.AddComponent<TestMonoBehaviourSelfInjection>();
			
			TestMonoBehaviourSelfInjection tmb = (TestMonoBehaviourSelfInjection)mb;
			
			Assert.That(tmb.testInjection.ReturnValue() == 5);
		}
	}
}
