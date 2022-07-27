Select distinct IdCuarto
From Reservas
Where IdCuarto NOT IN(
select IdCuarto 
from Reservas 
Where '7/9/2022 10:00:00' > FechaInicio AND '7/9/2022 10:00:00' < FechaFin
OR ('12/9/2022 10:00:00' > FechaInicio AND '12/9/2022 10:00:00' < FechaFin)
OR ('7/9/2022 10:00:00' < FechaInicio AND '12/9/2022 10:00:00' > FechaFin))