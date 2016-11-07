import matplotlib.pylab as plt
import numpy as np

#%%

def rosenbrock(x, y):
    "The Rosenbrock function for the contour plot calculation."

    return (1 - x)**2 + 100*(y - x**2)**2

# F1
X, Y = np.meshgrid(np.linspace(-10, 10, 100), np.linspace(-10, 10, 100))
Z = 100 * (Y - X ** 2) ** 2 + (1 - X) ** 2

fig = plt.figure()
f1plot = fig.add_subplot(211)
#f1plot.contourf(X, Y, Z)

xaxis = [-6, 6]
yaxis = xaxis

x_grid, y_grid = np.meshgrid(np.arange(xaxis[0], xaxis[1], 0.025), np.arange(yaxis[0], yaxis[1], 0.025))
z_grid = rosenbrock(x_grid, y_grid)
levels = [5, 25, 50, 100, 250, 500, 1000, 1500, 2000, 2500, 3000]

f1plot.contour(x_grid, y_grid, z_grid, levels)

#f1plot.set_xlim(-10, 10)
#f1plot.set_ylim(-10, 10)

plt.legend(loc='lower right')
plt.show()
