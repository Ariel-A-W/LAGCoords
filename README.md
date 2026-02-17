# LAGCoords - Optimizador de Rutas Logísticas

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=csharp)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-active-success)

Sistema avanzado de optimización de rutas logísticas basado en algoritmos genéticos para resolver el problema del viajante de comercio (TSP) con análisis de costos operacionales completo.

## 🎯 Características Principales

- **Algoritmo Genético Optimizado**: Implementación completa con operadores especializados para problemas de permutación
- **Cálculo Geográfico Preciso**: Distancias calculadas con la fórmula de Haversine para precisión sobre superficie esférica
- **Análisis de Costos Logísticos**: Desglose detallado de costos operacionales incluyendo combustible, peajes, salarios, depreciación y mantenimiento
- **Reportes Profesionales**: Generación automática de reportes con métricas de eficiencia y análisis por ciudad
- **Arquitectura Modular**: Diseño orientado a objetos con separación clara de responsabilidades

## 🚀 Tecnologías

- **Lenguaje**: C# 12.0
- **Framework**: .NET 10.0
- **Paradigma**: Computación Evolutiva / Metaheurísticas
- **Problema**: Traveling Salesman Problem (TSP)

## 📋 Requisitos Previos

- .NET SDK 10.0 o superior
- Visual Studio 2022+ o VS Code con extensión C#
- Sistema Operativo: Windows, Linux o macOS

## 🔧 Instalación

```bash
# Clonar el repositorio
git clone https://github.com/tu-usuario/LAGCoords.git

# Navegar al directorio del proyecto
cd LAGCoords/LAGCoords

# Restaurar dependencias
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar el proyecto
dotnet run
```

## 💡 Uso Básico

### Definir Ciudades

```csharp
var cities = new List<City>
{
    new City("Buenos Aires", -34.6037, -58.3816),
    new City("Córdoba", -31.4201, -64.1888),
    new City("Rosario", -32.9468, -60.6393),
    // ... más ciudades
};
```

### Ejecutar Optimización

```csharp
var ga = new GeneticAlgorithm(cities);
var best = ga.Run(
    populationSize: 200,
    generations: 500,
    mutationRate: 0.02
);
```

### Configurar Análisis de Costos

```csharp
var logisticConfig = new LogisticConfig
{
    FuelPricePerLiter = 950.0,      // ARS
    KmPerLiter = 8.0,               // Rendimiento
    AverageTollCost = 2500.0,       // ARS
    DriverHourlyCost = 3500.0,      // ARS/hora
    AverageSpeedKmH = 70.0          // km/h
};

var calculator = new LogisticCostCalculator(logisticConfig);
var costBreakdown = calculator.CalculateTotalCost(best.Route, totalDistance, stops);
```

## 🏗️ Arquitectura

El proyecto está organizado en las siguientes capas:

### Capa de Dominio
- **City**: Entidad que representa una ciudad con coordenadas geográficas
- **Individual**: Representa una solución (ruta) en el algoritmo genético
- **DistanceInfo**: Información detallada de distancias por ciudad
- **LogisticConfig**: Configuración de parámetros de costos

### Capa de Algoritmos Genéticos
- **GeneticAlgorithm**: Motor principal del algoritmo evolutivo
- **Selection**: Operador de selección por torneo
- **Crossover**: Operador de cruce ordenado (Order Crossover)
- **Mutation**: Operador de mutación por intercambio
- **FitnessCalculator**: Evaluación de calidad de soluciones

### Capa de Análisis
- **GeoDistance**: Cálculo de distancias con fórmula de Haversine
- **DistanceAnalyzer**: Análisis de métricas de distancia
- **LogisticCostCalculator**: Cálculo de costos operacionales
- **LogisticAnalytics**: Herramientas de análisis logístico

## 📊 Componentes Principales

### Algoritmo Genético

#### Operadores Implementados

**Selección por Torneo (Tournament Selection)**
```csharp
public static Individual Tournament(List<Individual> population, int size, Random rnd)
```
- Selecciona el mejor individuo de un subconjunto aleatorio
- Tamaño de torneo típico: 3
- Balance entre presión selectiva y diversidad

**Cruce Ordenado (Order Crossover - OX)**
```csharp
public static Individual OrderCrossover(Individual parent1, Individual parent2, Random rnd)
```
- Preserva orden relativo de genes
- Ideal para problemas de permutación
- Garantiza validez de soluciones

**Mutación por Intercambio (Swap Mutation)**
```csharp
public static void Swap(Individual individual, double rate, Random rnd)
```
- Intercambia aleatoriamente dos ciudades
- Tasa típica: 2% (0.02)
- Mantiene diversidad genética

### Cálculo de Fitness

La función de fitness se basa en la distancia total de la ruta:

```
Fitness = 1 / Distancia Total
```

Mayor fitness indica mejor solución (ruta más corta).

### Distancias Geográficas

Implementación de la fórmula de Haversine para cálculo preciso:

```csharp
public static double Between(City a, City b)
{
    // Conversión a radianes
    double lat1 = ToRad(a.Latitude);
    double lon1 = ToRad(a.Longitude);
    double lat2 = ToRad(b.Latitude);
    double lon2 = ToRad(b.Longitude);
    
    // Diferencias
    double dLat = lat2 - lat1;
    double dLon = lon2 - lon1;
    
    // Fórmula de Haversine
    double h = Math.Pow(Math.Sin(dLat / 2), 2) +
               Math.Cos(lat1) * Math.Cos(lat2) *
               Math.Pow(Math.Sin(dLon / 2), 2);
    
    double c = 2 * Math.Asin(Math.Sqrt(h));
    return EarthRadiusKm * c;
}
```

## 💰 Análisis de Costos Logísticos

El sistema calcula automáticamente:

- **Combustible**: Basado en consumo y precio por litro
- **Peajes**: Estimación según distancia entre peajes
- **Conductor**: Costo horario × tiempo de viaje
- **Paradas**: Costo fijo por carga/descarga
- **Depreciación**: Pérdida de valor por kilómetro
- **Mantenimiento**: Costo de mantenimiento por km
- **Costos Fijos**: Seguro, administración, overhead

### Desglose de Costos

```
DESGLOSE DE COSTOS:
─────────────────────────────────────────────────────
Combustible:                  $    12,345.67
Peajes:                       $     5,000.00
Conductor (tiempo):           $    15,678.90
Paradas/Descarga:            $    40,000.00
Depreciación vehículo:       $     8,900.00
Mantenimiento:               $     6,950.00
Costos fijos:                $    15,000.00
─────────────────────────────────────────────────────
TOTAL:                        $   103,874.57
Costo por km:                 $       52.45
```

## 📈 Rendimiento y Escalabilidad

### Complejidad Computacional

- **Complejidad por evaluación**: O(n) donde n = número de ciudades
- **Complejidad total**: O(p × g × n)
  - p = tamaño de población
  - g = número de generaciones
  - n = número de ciudades

### Tiempos de Ejecución Estimados

| Ciudades | Espacio de Búsqueda | Población | Generaciones | Tiempo |
|----------|---------------------|-----------|--------------|--------|
| 5        | 120                 | 100       | 200          | < 1s   |
| 8        | 40,320              | 200       | 500          | 1-3s   |
| 10       | 3.6M                | 300       | 1,000        | 5-10s  |
| 15       | 1.3T                | 500       | 2,000        | 30-60s |
| 20       | 2.4×10¹⁸           | 1,000     | 5,000        | 2-5min |

## 🔬 Parámetros de Configuración

### Parámetros del Algoritmo Genético

```csharp
// Configuración estándar
populationSize: 200     // Tamaño de la población
generations: 500        // Número de generaciones
mutationRate: 0.02      // Tasa de mutación (2%)
tournamentSize: 3       // Tamaño del torneo
```

### Ajuste de Parámetros

- **Población Grande** → Mayor diversidad, más tiempo de cómputo
- **Más Generaciones** → Mejor convergencia, mayor tiempo
- **Alta Mutación** → Más exploración, posible pérdida de convergencia
- **Baja Mutación** → Riesgo de convergencia prematura

## 📝 Ejemplo Completo

```csharp
using LAGCoords;

// 1. Definir ciudades argentinas
var cities = new List<City>
{
    new City("Buenos Aires", -34.6037, -58.3816),
    new City("Córdoba", -31.4201, -64.1888),
    new City("Rosario", -32.9468, -60.6393),
    new City("Mendoza", -32.8895, -68.8458),
    new City("La Plata", -34.9214, -57.9544),
    new City("San Miguel de Tucumán", -26.8083, -65.2176),
    new City("Mar del Plata", -38.0055, -57.5426),
    new City("Salta", -24.7859, -65.4117)
};

// 2. Ejecutar optimización
var ga = new GeneticAlgorithm(cities);
var best = ga.Run(
    populationSize: 200,
    generations: 500,
    mutationRate: 0.02
);

// 3. Calcular distancia total
double totalDistance = 1 / best.Fitness;
Console.WriteLine($"Distancia total: {totalDistance:F2} km");

// 4. Análisis de distancias
var distanceAnalyzer = new DistanceAnalyzer();
var distances = DistanceAnalyzer.CalculateDistancesFromStart(best.Route, cities[0]);
var accumulated = DistanceAnalyzer.CalculateAccumulatedDistances(best.Route);

// 5. Análisis de costos
var config = new LogisticConfig
{
    FuelPricePerLiter = 950.0,
    KmPerLiter = 8.0,
    AverageTollCost = 2500.0
};

var calculator = new LogisticCostCalculator(config);
var costs = calculator.CalculateTotalCost(best.Route, totalDistance, cities.Count);

// 6. Mostrar resultados
Console.WriteLine($"Costo total: ${costs.TotalCost:F2}");
Console.WriteLine($"Costo por km: ${costs.CostPerKm:F2}");
Console.WriteLine($"Eficiencia: {totalDistance / costs.TotalCost * 1000:F2} km/$1000");
```

## 🧪 Testing

Para ejecutar pruebas (cuando estén implementadas):

```bash
dotnet test
```

## 🛠️ Posibles Extensiones

### Mejoras Algorítmicas
- Implementar elitismo para preservar mejores soluciones
- Añadir búsqueda local híbrida (2-opt, 3-opt)
- Criterios de parada adaptativos
- Operadores genéticos adicionales (PMX, ERX)
- Adaptación dinámica de parámetros

### Extensiones Funcionales
- Múltiples vehículos (Vehicle Routing Problem)
- Ventanas temporales de entrega
- Restricciones de capacidad
- Costos variables por segmento
- Optimización multi-objetivo (NSGA-II)
- Persistencia en base de datos
- Interfaz gráfica de usuario
- API REST para integración

## 📖 Documentación Adicional

- **Análisis Técnico Completo**: Ver `docs/analisis_tecnico_lagcoords.pdf`
- **Diagramas de Arquitectura**: Ver carpeta `docs/diagrams/`
- **Referencias Académicas**: Ver sección de referencias en documentación

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver archivo `LICENSE` para más detalles.

## ✨ Autores

- **Desarrollador Principal** - [Tu Nombre]

## 🙏 Agradecimientos

- Inspirado en el trabajo fundamental de John Holland sobre algoritmos genéticos
- Basado en las técnicas de Order Crossover de Davis (1985)
- Implementación de Haversine para cálculos geográficos precisos

## 📞 Contacto

- Email: arqdev.arielwagner@gmail.com
- LinkedIn: [Ariel Alejandro Wagner](https://www.linkedin.com/in/ariel-alejandro-w-a4834075/?locale=en_US) 
- Blog: [Medium](https://tu-blog.com)

---

**LAGCoords** - Optimización inteligente de rutas logísticas mediante algoritmos genéticos

*Desarrollado con ❤️ y algoritmos evolutivos*
