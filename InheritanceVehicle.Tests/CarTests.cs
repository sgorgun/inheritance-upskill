using System.Reflection;
using NUnit.Framework;

namespace InheritanceVehicle.Tests
{
    [TestFixture]
    public class CarTests
    {
        private const string CarClassName = "car";
        private const string VehicleClassName = "vehicle";
        private Type carType;

        [SetUp]
        public void Initialize()
        {
            var assembly = typeof(Stub).Assembly;
            this.carType = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(CarClassName, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void Car_Class_Is_Created()
        {
            Assert.IsNotNull(this.carType, "'Car' class is not created.");
        }

        [Test]
        public void Car_Inherits_Vehicle()
        {
            var carInstance = Activator.CreateInstance(this.carType, string.Empty, 0);
            var vehicleType = typeof(Stub).Assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(VehicleClassName, StringComparison.OrdinalIgnoreCase));

            Assert.IsInstanceOf(vehicleType, carInstance, "'Car' type does NOT inherit 'Vehicle' type.");
        }

        [Test]
        public void Set_Name_Method_Is_Defined()
        {
            var method = this.carType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    var parameters = m.GetParameters();
                    if (m.ReturnType == typeof(void) && parameters?.FirstOrDefault()?.ParameterType == typeof(string))
                    {
                        return true;
                    }

                    return false;
                });

            Assert.IsNotNull(method, "Method which changes 'Car' name is NOT define or it does NOT contain correct parameters.");
        }

        [Test]
        public void Get_Name_Method_Is_Defined()
        {
            var method = this.carType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    if (m.ReturnType == typeof(string) && m.GetParameters().Length == 0)
                    {
                        return true;
                    }

                    return false;
                });

            Assert.IsNotNull(method, "Method which retrieves 'Car' name is NOT define or it is's return type is NOT correct.");
        }

        [Test]
        public void Get_Car_Name()
        {
            var name = "Toyota";
            var newName = "BMW";
            var age = 5;
            var carInstance = Activator.CreateInstance(this.carType, name, age);

            var setNameMethod = this.carType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    var parameters = m.GetParameters();
                    if (m.ReturnType == typeof(void) && parameters?.FirstOrDefault()?.ParameterType == typeof(string))
                    {
                        return true;
                    }

                    return false;
                });

            var getNameMethod = this.carType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    if (m.ReturnType == typeof(string) && m.GetParameters().Length == 0)
                    {
                        return true;
                    }

                    return false;
                });

            setNameMethod.Invoke(carInstance, new[] { newName });

            var carName = getNameMethod.Invoke(carInstance, Array.Empty<object>());

            Assert.AreEqual(
                newName,
                carName,
                $"'{getNameMethod.Name}' method does NOT return correct value or '{setNameMethod.Name}' method does NOT change car name correctly.");
        }

        [Test]
        public void TestCarStart()
        {
            // Arrange
            Car car = new Car("TestCar", 100);

            // Act
            car.CarStart();

            // Assert
            Assert.IsTrue(car.IsStarted, "Car should be started.");
        }

        [Test]
        public void TestCarStop()
        {
            // Arrange
            Car car = new Car("TestCar", 100);
            car.CarStart();

            // Act
            car.CarStop();

            // Assert
            Assert.IsFalse(car.IsStarted, "Car should be stopped.");
            Assert.IsFalse(car.IsDriving, "Car should not be driving.");
            Assert.AreEqual(0, car.Speed, "Car speed should be 0.");
        }

        [Test]
        public void TestCarDrive_WhenCarStarted()
        {
            // Arrange
            Car car = new Car("TestCar", 100);
            car.CarStart();

            // Act
            car.CarDrive();

            // Assert
            Assert.IsTrue(car.IsDriving, "Car should be driving.");
        }

        [Test]
        public void TestCarDrive_WhenCarNotStarted()
        {
            // Arrange
            Car car = new Car("TestCar", 100);

            // Act
            car.CarDrive();

            // Assert
            Assert.IsFalse(car.IsDriving, "Car should not be driving.");
        }

        [Test]
        public void TestCarAccelerate_WhenCarStartedAndDriving()
        {
            // Arrange
            Car car = new Car("TestCar", 100);
            car.CarStart();
            car.CarDrive();

            // Act
            car.CarAccelerate();

            // Assert
            Assert.IsTrue(car.Speed > 0, "Car speed should be greater than 0.");
        }

        [Test]
        public void TestCarAccelerate_WhenCarNotStarted()
        {
            // Arrange
            Car car = new Car("TestCar", 100);

            // Act
            car.CarAccelerate();

            // Assert
            Assert.AreEqual(0, car.Speed, "Car speed should be 0.");
        }

        [Test]
        public void TestCarDecelerate_WhenCarStartedAndDriving()
        {
            // Arrange
            Car car = new Car("TestCar", 100);
            car.CarStart();
            car.CarDrive();
            car.CarAccelerate();

            // Act
            car.CarDecelerate();

            // Assert
            Assert.IsTrue(car.Speed >= 0, "Car speed should be greater than or equal to 0.");
        }

        [Test]
        public void TestCarDecelerate_WhenCarNotStarted()
        {
            // Arrange
            Car car = new Car("TestCar", 100);

            // Act
            car.CarDecelerate();

            // Assert
            Assert.AreEqual(0, car.Speed, "Car speed should be 0.");
        }
    }
}
